using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Slots))]
[RequireComponent(typeof(LootTable))]
public class Pool : Singleton<Pool> {
    private LootTable lootTable;
    private Slots poolSlots;

    public ZoneEvent poolZoneEvent;
    public Card cardPrefab;
    public GameObject origin;

    void Start() {
        if (poolZoneEvent == null) {
            poolZoneEvent = new ZoneEvent();
        }

        poolSlots = GetComponent<Slots>();
        lootTable = GetComponent<LootTable>();

        poolSlots.claimEvent.AddListener(OnClaimSlot);
        poolSlots.releaseEvent.AddListener(OnReleaseSlot);
    }

    public ObjectSlot ClaimASlot(Card card) {
        return poolSlots.ClaimASlot(card.gameObject);
    }

    public void DrawToPool() {
        if(poolSlots.anyOpenSlots) {
            var cardModel = lootTable.PullCard();
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
