using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInLane : MonoBehaviour
{
    private Owner owner;
    private CardModel card;
    private ObjectSlot slot;

    public void SetOwner(Owner owner) {
        if(owner == Owner.None) {
            Debug.LogError("Attempting to set a CardInLane owner to 'NONE'");
        }

        this.owner = owner;
    }

    public void SetCardModel(CardModel newCard)
    {
        card = newCard;
    }

    // Returns health after damage taken
    public int TakeDamage(int damage)
    {
        card.health -= damage;

        return card.health;
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

    public void RemoveFromPlay()
    {
        Destroy(gameObject);
        // TODO: Put back in discard pile
    }
}
