using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardInLaneView))]
public class CardInLane : MonoBehaviour {

    private CardInLaneView cardView;

    public Owner owner { get; private set; } 
    public CardModel card { get; private set; }
    public ObjectSlot slot { get; private set; }
    public int timeToCast;
    public Card.CardType cardType;

    public int currHp { get; private set; }

    public void SetOwner(Owner owner) {
        if(owner == Owner.None) {
            Debug.LogError("Attempting to set a CardInLane owner to 'NONE'");
        }

        this.owner = owner;
    }

    public void Start() {
        cardView = GetComponent<CardInLaneView>();
    }

    public void SetCardModel(CardModel newCard)
    {
        cardView = GetComponent<CardInLaneView>(); //TODO must be a better option than this...
        card = newCard;
        currHp = newCard.health;
        cardView.CreateCardImage(this);
    }

    // Returns health after damage taken
    public int TakeDamage(int damage) {
        currHp -= damage;
        cardView.CreateCardImage(this);
        return currHp;
    }

    // Returns damage card can deal
    public int GetAttackDamage()
    {
        return card.damage;
    }

    public void SetSlot(ObjectSlot currSlot)
    {
        slot = currSlot;
    }
}
