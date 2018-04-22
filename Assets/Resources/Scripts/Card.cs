using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour {

    public enum State { None, InDeck, InPool, InHand, InLane, MovingToPool, MovingToHand, MovingToDiscard, Playing }

    public State prevState = State.None;
    public State state = State.None;
    public CardModel cardModel = null;
    public CardView cardView = null;

    public ObjectSlot currSlot = null;
    public float scaleTarget;
    public float speed = 30f;
    public float scaleDuration = 0.5f;

    private void Start() {
        cardView = GetComponent<CardView>(); 
    }

    void Update () {
        bool arrived = false;
	    switch(state) {
            case State.MovingToPool:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if (arrived) OnMovedToPool();
                break;

            case State.MovingToDiscard:
                arrived = UpdateMoveTo(Deck.instance.discard.transform.position);
                if (arrived) OnMovedToDiscard();
                break;

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
        //TODO update CARD PREFAB later 
        this.cardView = GetComponent<CardView>();
        this.cardView.CreateCardImage(cardModel);
    }

    public void SetInLane() {
        ChangeState(State.InLane);
    }

    public void SetInPool() {
        ChangeState(State.InPool);
        //TODO any clean-up if this happens suddenly
    }

    public void SetInDeck() {
        ChangeState(State.InDeck);
        Deck.instance.ConfigureCardObjectAtDeck(this);
    }

    public void MoveToPool() {
        if (!AssertState(State.None)) return;

        var nextSlot = Pool.instance.ClaimASlot(this);
        if(nextSlot != null) {
            OnMovingToPool();

            currSlot = nextSlot;
            ChangeState(State.MovingToPool);
        } else {
            Debug.Log("Tried to move to pool but it was full!");
            Bail();
        }
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
            Bail();
        }
    }

    public void Play() {
        if (!AssertState(State.InHand)) return;

        var nextSlot = LaneManager.instance.ClaimPlayerSlot(gameObject);
        if (nextSlot != null) {
            OnPlaying();

            currSlot = nextSlot;
            ChangeState(State.Playing);
        } else {
            OnNoRoomForCard();
        }
    }

    public void Discard() {
        if (!AssertState(State.MovingToPool, State.InPool, State.InLane)) return;

        if(currSlot) {
            currSlot.Release();
            currSlot = null;
        }

        //TODO start scaling down to discard size?

        ChangeState(State.MovingToDiscard);
    }

    private void OnMovingToPool() {
        transform.position = Pool.instance.origin.transform.position;
    }

    private void OnMovedToPool() {
        currSlot.Occupy(this.gameObject);
        ChangeState(State.InPool);
    }

    private void OnMovedToDiscard() {
        Deck.instance.Discard(this);
        Destroy(this.gameObject);
    }

    private void OnMovedToHand() {
        if (!AssertState(State.MovingToHand)) return;

        currSlot.Occupy(this.gameObject); 
        ChangeState(State.InHand);
    }

    private void OnPlaying() {
        currSlot.Release();
        currSlot = null;

        //TODO any inital set or tear down from being in the hand (deregister from input manager)
    }

    private void OnPlayed() {
        if (!AssertState(State.Playing)) return;

        //TODO create game prefab for lane object;
        if(cardModel.prototype.inLanePrefab != null) {
            GameObject laneObject = GameObject.Instantiate(cardModel.prototype.inLanePrefab);
            
            laneObject.transform.position = currSlot.transform.position;
            //laneObject.SetCardModel(cardModel);
            //laneObject.SetSlot(currSlot);

            currSlot.Occupy(laneObject);

            //TODO sound effects or whatever (maybe configed on card prototype)
        } else {
            currSlot.Release();
            currSlot = null;
        }

        Destroy(gameObject);
    }

    private void OnNoRoomForCard() {
        //TODO what happens when there's no room for the card?
    }

    private void ChangeState(State state) {
        prevState = this.state;
        this.state = state;
    }

    private bool AssertState(params State[] expecteds) {
        var assert = expecteds.Any(expected => state == expected);
        if(!assert) {
            var expectedList = string.Join(",", Array.ConvertAll(expecteds, e => e.ToString())); 
            Debug.Log(string.Format("Expected to be in state '{0}', but state was '{1}'", expectedList, state));
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

    private void Bail() {
        ChangeState(State.None);
        //TODO in release version, just destroy?
    }
}
