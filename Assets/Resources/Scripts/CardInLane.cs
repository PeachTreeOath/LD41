using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardInLaneView))]
public class CardInLane : MonoBehaviour
{

    private CardInLaneView cardView;

    public Owner owner { get; private set; }
    public CardModel card { get; private set; }
    public ObjectSlot slot { get; private set; }
    public int timeToCast { get; protected set; }
    public Card.CardType cardType; //TODO should probably have been in card prototype...

    public int currHp { get; private set; }

    public void SetOwner(Owner owner)
    {
        if (owner == Owner.None)
        {
            Debug.LogError("Attempting to set a CardInLane owner to 'NONE'");
        }

        this.owner = owner;
        CardAttackBehavior attackBehavior = GetComponent<CardAttackBehavior>();
        if (owner == Owner.Enemy && attackBehavior != null)
        {
            // Reverse direction
            attackBehavior.positionDelta = new Vector3(attackBehavior.positionDelta.x, -attackBehavior.positionDelta.y, attackBehavior.positionDelta.z);
        }
    }

    public void Start()
    {
        cardView = GetComponent<CardInLaneView>();
    }

    public void SetCardModel(CardModel newCard)
    {
        cardView = GetComponent<CardInLaneView>(); //TODO must be a better option than this...
        card = newCard;
        currHp = newCard.health;
        timeToCast = newCard.timeToCast;
        cardView.CreateCardImage(card, owner, currHp); //TODO update the view with data from the card image?
    }

    // Returns health after damage taken
    public int TakeDamage(int damage)
    {
        currHp -= damage;
        cardView.CreateCardImage(card, owner, currHp);
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
