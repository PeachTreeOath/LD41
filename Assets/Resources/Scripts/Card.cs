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
	    switch(state) {
            case State.MovingToHand:
                var arrived = UpdateMoveTo(currSlot.transform.position);
                if(arrived) OnMovedToHand();
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

    private void OnMovedToHand() {
        if (!AssertState(State.MovingToHand)) return;

        currSlot.Occupy(this.gameObject); //TODO is the trigger here or on the slot callback?  Probably slot callback
        ChangeState(State.InHand);
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
