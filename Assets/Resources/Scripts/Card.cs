﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour {

    public enum State { None, InDeck, InPool, InHand, InLane, MovingToHand, MovingToDiscard, Playing }

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
        if (!AssertState(State.InPool, State.InLane)) return;

        switch(state) {
            case State.InPool:

                break;
        }

        ChangeState(State.MovingToDiscard);
    }

    private void OnMovedToHand() {
        if (!AssertState(State.MovingToHand)) return;

        currSlot.Occupy(this.gameObject); //TODO is the trigger here or on the slot callback?  Probably slot callback
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
            CardInLane card = laneObject.GetComponent<CardInLane>();
            card.SetCardModel(cardModel);
            card.SetSlot(currSlot);

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
}
