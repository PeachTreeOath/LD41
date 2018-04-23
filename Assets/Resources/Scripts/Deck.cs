using UnityEngine;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(DeckModel))]
public class Deck : Singleton<Deck> {
    private DeckModel model;
    private Slots discardSlots;

    public Card cardPrefab;
    public GameObject discard;

    public TextMeshProUGUI deckText;
    public TextMeshProUGUI discardText;

    public void Start() {
        model = GetComponent<DeckModel>();

        discardSlots = GetComponent<Slots>();
        discardSlots.occupyEvent.AddListener(OnOccupyDiscardSlot);
        discardSlots.releaseEvent.AddListener(OnReleaseDiscardSlot);

        deckText.text = model.library.Count.ToString();
        discardText.text = model.discard.Count.ToString();

        HideDeck();
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

            UpdateCounts();

            return true;
        } else {
            OnNoCardsLeft();
            return false;
        }
    }

    public void Discard(Card card) {
        model.Discard(card.cardModel);
        Clear();
        discardSlots.ClaimASlot(card.gameObject);
        UpdateCounts();
    }

    public void Clear() {
        if(!discardSlots.IsOpenAt(0)){
            discardSlots.slots[0].Release();
        }
    }

    void OnCardCreated(CardModel cardModel, Card card) {
        card.SetCardModel(cardModel);
        card.SetOwner(Owner.Player);
        card.SetInDeck();
        card.MoveToHand();
        if(model.library.Count > 0)
        {
            ShowDeck();
        }
        else
        {
            HideDeck();
        }
    }

    void OnHandFull() {

    }

    void OnNoCardsLeft() {

    }

    void ShowDeck() {
        transform.GetComponent<SpriteRenderer>().enabled = true;
    }

    void HideDeck() {
        transform.GetComponent<SpriteRenderer>().enabled = false;
    }

    void UpdateCounts() {
        deckText.text = model.library.Count.ToString();
        discardText.text = model.discard.Count.ToString();
    }

    private void OnOccupyDiscardSlot(ObjectSlot slot, GameObject go) {
        //Show the gameobject
    }

    private void OnReleaseDiscardSlot(ObjectSlot slot, GameObject go) {
        Destroy(go);
    }

}
