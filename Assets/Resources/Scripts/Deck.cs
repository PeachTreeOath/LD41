using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(DeckModel))]
public class Deck : Singleton<Deck> {
    private DeckModel model;

    public Card cardPrefab;
    public GameObject discard;

    public void Start() {
        model = GetComponent<DeckModel>();
    }

    public void ConfigureCardObjectAtDeck(Card card) {
        card.transform.localScale = new Vector4(0.5f, 0.5f, 1f);
        card.transform.position = transform.position;
    }

    public bool Draw() {
        if(!Hand.instance.canDrawCards) {
            OnHandFull();
            return false;
        }

        CardModel cardModel = model.Draw();
        if(cardModel != null) {
            Card card = GameObject.Instantiate<Card>(cardPrefab);
            OnCardCreated(cardModel, card);

            return true;
        } else {
            OnNoCardsLeft();
            return false;
        }
    }

    public void Discard(Card card) {
        model.Discard(card.cardModel);
    }

    void OnCardCreated(CardModel cardModel, Card card) {
        card.SetCardModel(cardModel); 
        card.SetInDeck();
        card.MoveToHand();
    }

    void OnHandFull() {

    }
    
    void OnNoCardsLeft() {

    }

}
