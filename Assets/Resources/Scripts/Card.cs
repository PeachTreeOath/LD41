using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {

    public enum State { None, InDeck, InPool, InHand, MovingToHand, MovingToDiscard, Playing }

    public State prevState = State.None;
    public State state = State.None;
    public CardModel cardModel = null;

    public ObjectSlot currSlot = null;
    public float scaleTarget;
    public float speed = 30f;
    public float scaleDuration = 0.5f;

	void Update () {
        bool arrived = false;
	    switch(state) {
            case State.MovingToHand:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if(arrived) OnMovedToHand();
                break;

            case State.Playing:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if (arrived) OnPlayed();
                break;
        }	
	}

    public void SetCardModel(CardModel cardModel) {
        this.cardModel = cardModel;
        //TODO update visuals!
    }

    public void SetInPool() {
        state = State.InPool;
        //TODO any clean-up if this happens suddenly
    }

    public void SetInDeck() {
        ChangeState(State.InDeck);
        Deck.instance.ConfigureCardObjectAtDeck(this);
    }

    public void MoveToHand() {
        if (state == State.InHand) {
            Debug.Log("Cannot move to hand, card is already in hand");
            return;
        }

        var nextSlot = Hand.instance.ClaimASlot(this);
        if(nextSlot != null) {
            switch(state) {
                case State.InPool:
                    //TODO initialize for moving from pool
                    break;
                case State.InDeck: break;
                    //TODO initialize for moving from deck
                default:
                    //Dunno what's going on-- force initial settings
                    break;
            }

            currSlot = nextSlot;
            ChangeState(State.MovingToHand);
        } else {
            Debug.LogError("Tried to move to hand but the hand was full!");
            ChangeState(State.None);
        }
    }

    public void Play() {
        if (!AssertState(State.InHand)) return;

        //var nextSlot = Lanes.instance.ClaimASlot(this);
        ObjectSlot nextSlot = null; //TODO replace with equivalent of above
        if (nextSlot != null) {
            //TODO any inital set or tear down from being in the hand (deregister from input manager)

            currSlot = nextSlot;
            ChangeState(State.Playing);
        } else {
            OnNoRoomForCard();
        }
    }

    private void OnMovedToHand() {
        if (!AssertState(State.MovingToHand)) return;

        currSlot.Occupy(this.gameObject); //TODO is the trigger here or on the slot callback?  Probably slot callback
        ChangeState(State.InHand);
    }

    private void OnPlayed() {
        if (!AssertState(State.Playing)) return;

        //TODO create game prefab for lane object;
        GameObject laneObject = null;
        laneObject.transform.position = currSlot.transform.position;
        //laneObject.SetCardModel(cardModel);
        //laneObject.SetSlot(currSlot);

        currSlot.Occupy(laneObject);

        //TODO sound effects or whatever (maybe configed on card prototype)

        Destroy(gameObject);
    }

    private void OnNoRoomForCard() {
        //TODO what happens when there's no room for the card?
    }

    private void ChangeState(State state) {
        prevState = this.state;
        this.state = state;
    }

    private bool AssertState(State expected) {
        var assert = state == expected;
        if(!assert) {
            Debug.Log(string.Format("Expected to be in state '{0}', but state was '{1}'", expected, state));
            state = State.None;
        }

        return assert;
    }

    private bool UpdateMoveTo(Vector2 destination) {
        var step = Time.deltaTime * speed;

        var curr = new Vector2(transform.position.x, transform.position.y);
        var vec = destination - curr;
        var dist = vec.magnitude;

        if(step > dist) {
            transform.position = destination;
            return true;
        } else {
            vec = vec.normalized * step;
            transform.position = transform.position + new Vector3(vec.x, vec.y, 0);

            return false;
        }
    }
}
