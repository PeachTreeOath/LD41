using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(CardView))]
public class Card : MonoBehaviour
{
    public enum State { None, InDeck, InPool, InHand, InLane, MovingToPool, MovingToHand, MovingToDiscard, Playing }
    public enum CardType { Monster, Spell }

    public State prevState = State.None;
    public State state = State.None;
    public Owner owner = Owner.None;

    public CardModel cardModel = null;
    public CardView cardView = null;
    public CardType cardType = CardType.Monster;

    public ObjectSlot currSlot = null;
    public float scaleTarget;
    public float speed = 30f;
    public float scaleDuration = 0.5f;

    private void Start()
    {
        cardView = GetComponent<CardView>();
    }

    void Update()
    {
        bool arrived = false;
        switch (state)
        {
            case State.MovingToPool:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if (arrived) OnMovedToPool();
                break;

            case State.MovingToDiscard:
                Vector2 target;
                if(owner == Owner.Player) {
                    target = Deck.instance.discard.transform.position;
                } else {
                    target = Enemy.instance.discard.transform.position;
                }

                arrived = UpdateMoveTo(target);
                if (arrived) OnMovedToDiscard();
                break;

            case State.MovingToHand:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if (arrived) OnMovedToHand();
                break;

            case State.Playing:
                arrived = UpdateMoveTo(currSlot.transform.position);
                if (arrived) OnPlayed();
                break;
        }
    }

    public void SetCardModel(CardModel cardModel)
    {
        this.cardModel = cardModel;
        if (cardModel.timeToCast >= 0)
        {
            cardType = CardType.Spell;
        }

        //TODO update CARD PREFAB later 
        this.cardView = GetComponent<CardView>();
        this.cardView.CreateCardImage(cardModel);
    }

    public void SetOwner(Owner newOwner)
    {
        if (owner != Owner.None)
        {
            Debug.Log(String.Format("Changing the owner of card from '{0}' to '{1}'", this.owner, newOwner));
        }

        this.owner = newOwner;
    }

    public void SetInLane()
    {
        ChangeState(State.InLane);
    }

    public void SetInPool()
    {
        ChangeState(State.InPool);
        //TODO any clean-up if this happens suddenly
    }

    public void SetInDeck()
    {
        ChangeState(State.InDeck);
        Deck.instance.ConfigureCardObjectAtDeck(this);
    }

    public void MoveToPool()
    {
        if (!AssertState(State.None) || !AssertOwner(Owner.None)) return;

        var nextSlot = Pool.instance.ClaimASlot(this);
        if (nextSlot != null)
        {
            OnMovingToPool();

            currSlot = nextSlot;
            ChangeState(State.MovingToPool);
        }
        else
        {
            Debug.Log("Tried to move to pool but it was full!");
            Bail();
        }
    }

    public void MoveToHand()
    {
        if (state == State.InHand)
        {
            Debug.Log("Cannot move to hand, card is already in hand");
            return;
        }

        var nextSlot = Hand.instance.ClaimASlot(this);
        if (nextSlot != null)
        {
            switch (state)
            {
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
        }
        else
        {
            Debug.LogError("Tried to move to hand but the hand was full!");
            Bail();
        }
    }

    public void Play(int desiredIndex = -1, bool failIfNotOpen = false)
    {
        if (!AssertOwner(Owner.Player, Owner.Enemy)) return;

        ObjectSlot nextSlot = null;
        if (owner == Owner.Player)
        {
            if (!AssertState(State.InHand)) return;
            nextSlot = LaneManager.instance.ClaimPlayerSlot(gameObject);
        }
        else
        {
            nextSlot = LaneManager.instance.ClaimEnemySlot(gameObject, desiredIndex, failIfNotOpen);
        }

        if (nextSlot != null)
        {
            OnPlaying();

            currSlot = nextSlot;
            ChangeState(State.Playing);
        }
        else
        {
            OnNoRoomForCard();
        }
    }

    public void Discard()
    {
        if (!AssertState(State.MovingToPool, State.InPool, State.InLane)) return;

        if (currSlot)
        {
            currSlot.Release();
            currSlot = null;
        }

        //TODO start scaling down to discard size?

        ChangeState(State.MovingToDiscard);
    }

    private void OnMovingToPool()
    {
        transform.position = Pool.instance.origin.transform.position;
    }

    private void OnMovedToPool()
    {
        currSlot.Occupy(this.gameObject);
        ChangeState(State.InPool);
    }

    private void OnMovedToDiscard()
    {
        if (owner == Owner.Player)
        {
            Deck.instance.Discard(this);
        }

        Destroy(this.gameObject);
    }

    private void OnMovedToHand()
    {
        if (!AssertState(State.MovingToHand)) return;

        currSlot.Occupy(this.gameObject);
        ChangeState(State.InHand);
    }

    private void OnPlaying()
    {
        if (currSlot != null)
        {
            currSlot.Release();
            currSlot = null;
        }

        //TODO any inital set or tear down from being in the hand (deregister from input manager)
    }

    private void OnPlayed()
    {
        if (!AssertState(State.Playing)) return;

        if (cardModel.prototype.inLanePrefab != null)
        {
            GameObject laneObject = GameObject.Instantiate(cardModel.prototype.inLanePrefab);

            laneObject.transform.position = currSlot.transform.position;
            CardInLane card = laneObject.GetComponent<CardInLane>();
            card.SetOwner(owner);
            card.SetCardModel(cardModel);
            card.SetSlot(currSlot);
            card.cardType = cardType;
            card.timeToCast = cardModel.timeToCast;

            currSlot.Occupy(laneObject);

            //TODO sound effects or whatever (maybe configed on card prototype)
        }
        else
        {
            currSlot.Release();
            currSlot = null;
        }

        Destroy(gameObject);
    }

    private void OnNoRoomForCard()
    {
        //TODO what happens when there's no room for the card?
    }

    private void ChangeState(State state)
    {
        prevState = this.state;
        this.state = state;
    }

    private bool AssertState(params State[] expecteds)
    {
        var assert = expecteds.Any(expected => state == expected);
        if (!assert)
        {
            var expectedList = string.Join(",", Array.ConvertAll(expecteds, e => e.ToString()));
            Debug.Log(string.Format("Expected to be in state '{0}', but state was '{1}'", expectedList, state));
            state = State.None;
        }

        return assert;
    }

    private bool AssertOwner(params Owner[] expecteds)
    {
        var assert = expecteds.Any(expected => owner == expected);
        if (!assert)
        {
            var expectedList = string.Join(",", Array.ConvertAll(expecteds, e => e.ToString()));
            Debug.Log(string.Format("Expected owner to be '{0}', but was '{1}'", expectedList, owner));
            state = State.None;
        }

        return assert;
    }

    private bool UpdateMoveTo(Vector2 destination)
    {
        var step = Time.deltaTime * speed;

        var curr = new Vector2(transform.position.x, transform.position.y);
        var vec = destination - curr;
        var dist = vec.magnitude;

        if (step > dist)
        {
            transform.position = destination;
            return true;
        }
        else
        {
            vec = vec.normalized * step;
            transform.position = transform.position + new Vector3(vec.x, vec.y, 0);

            return false;
        }
    }

    private void Bail()
    {
        ChangeState(State.None);
        //TODO in release version, just destroy?
    }

    public void MatchedWord(Owner actor = Owner.Player)
    {
        //Can be in either Hand or Pool when a match happens
        if (state == State.InHand)
        {
            if (owner == Owner.Player)
            {
                Play();
            }
        }
        else if (state == State.InPool)
        {
            SetOwner(actor);
            Discard();
        }
    }
}
