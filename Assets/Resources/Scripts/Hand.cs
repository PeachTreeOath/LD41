using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Hand : Singleton<Hand> {

    private Slots slots;

    public GameObject cardPrefab;

    public bool canDrawCards {  get { return slots.anyOpenSlots;  } }

    public void Start() {
        slots = GetComponent<Slots>();
    }

    public ObjectSlot ClaimASlot(Card cardObject) {
        return slots.ClaimASlot(cardObject.gameObject); //TODO this is sooo wonky...
    }

    public bool DrawFromDeck() {
        //Card card = Deck.instance.Draw();
        //if(card != null) {
            
            //GameObject cardGo = Instantiate(cardPrefab);
            //cardGo.GetComponent<CardView>().CreateCardImage(card);
            //TODO tell card class to position "over" deck
            //TODO slide the card over to the hand

            //cards.Add(card);

            return true;
        //}

        //return false;
    } 
}
