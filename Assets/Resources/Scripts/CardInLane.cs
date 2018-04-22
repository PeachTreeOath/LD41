using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInLane {

    private CardModel card;
    private ObjectSlot slot;

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
        card.health -= damage;

        return card.health;
    }
    public void SetSlot(ObjectSlot currSlot)
    {
        slot = currSlot;
    }
}
