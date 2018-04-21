using UnityEngine;
using System.Collections.Generic;

public class Hand : Singleton<Hand> {
    public GameObject cardPrefab;

    public List<Card> cards = new List<Card>();
    public int maxHandSize = 5;

    public bool canDrawCards {  get { return cards.Count < maxHandSize; } }
    
    public bool DrawFromDeck() {
        Card card = Deck.instance.Draw();
        if(card != null) {
            
            GameObject cardGo = Instantiate(cardPrefab);
            cardGo.GetComponent<CardView>().CreateCardImage(card);
            //TODO tell card class to position "over" deck
            //TODO slide the card over to the hand

            cards.Add(card);

            return true;
        }

        return false;
    } 
}
