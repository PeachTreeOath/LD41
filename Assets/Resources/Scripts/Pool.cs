using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Slots))]
[RequireComponent(typeof(DeckModel))]
public class Pool : Singleton<Pool> {
    private Slots poolSlots;
    private DeckModel poolDeck;
	private bool shouldShuffle = true;
	private bool isShuffled = false;

    public ZoneEvent poolZoneEvent;
    public Card cardPrefab;
    public GameObject origin;

    void Start() {
        if (poolZoneEvent == null) {
            poolZoneEvent = new ZoneEvent();
        }

        poolSlots = GetComponent<Slots>();
        poolDeck = GetComponent<DeckModel>();

        poolSlots.claimEvent.AddListener(OnClaimSlot);
        poolSlots.releaseEvent.AddListener(OnReleaseSlot);
    }

    void Update() {
        if (!Input.GetKey(KeyCode.LeftShift)) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            BuyFromSlot(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            BuyFromSlot(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            BuyFromSlot(2);
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            BuyFromSlot(3);
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            BuyFromSlot(4);
        } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            BuyFromSlot(5);
        }
    }

    //TODO remove when typing works
    void BuyFromSlot(int index) {
        var slot = poolSlots.slots[index];
        if(!slot.open) {
            var card = slot.objectInSlot.GetComponent<Card>();
            card.SetOwner(Owner.Player);
            card.Discard();
        }
    }

    public ObjectSlot ClaimASlot(Card card) {
        return poolSlots.ClaimASlot(card.gameObject);
    }

    //TODO set up some component to schedule these
    public void DrawToPool(bool reshuffle=false) {
        //Shuffle when the first card is pulled
		if(reshuffle || (shouldShuffle && !isShuffled)) {
            poolDeck.Shuffle();
			isShuffled = true;
		}

        if(poolSlots.anyOpenSlots) {
            var cardModel = poolDeck.Draw();
            if(cardModel != null) {
                var card = GameObject.Instantiate(cardPrefab);
                card.SetCardModel(cardModel);
                card.MoveToPool();
            }
        } else {
            Debug.Log("No Open Pool slots!");
        }
    }

    public bool DrawFromPool(Card card) {
        var foundCard = false;
        foreach(var slot in poolSlots.slots) {
            var otherCard = slot.GetComponent<Card>();
            if(otherCard == card) {
                foundCard = true;
                card.Discard(); 
            }
        }

        return foundCard;
    }

    private void OnClaimSlot(ObjectSlot slot, GameObject addedToPool) {
        poolZoneEvent.Invoke(ZoneEvent.ENTERED, addedToPool.GetComponent<Card>());
    }

    private void OnReleaseSlot(ObjectSlot slot, GameObject removedFromPool) {
        poolZoneEvent.Invoke(ZoneEvent.EXITED, removedFromPool.GetComponent<Card>());
    }
}
