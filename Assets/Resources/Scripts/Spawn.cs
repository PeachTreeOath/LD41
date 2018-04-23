using UnityEngine;
using System.Collections.Generic;
using System;

public enum SlotSelectionStrategy { Random, PreferUnopposed, BlockHighestDamage, KillWeakest }

[Serializable]
public class SpawnComponent {
    public CardPrototype prototype;
    public SlotSelectionStrategy slotSelectionStrategy = SlotSelectionStrategy.Random;
    public int multiplicity = 1;
    public float successRate = 1f;

    public void Execute() {
        var times = Math.Max(1, multiplicity);
        for(var i = 0; i < times; i++) {
            if (UnityEngine.Random.value > successRate) continue;

            if (!LaneManager.instance.enemySlots.anyOpenSlots) return;

            var cardModel = prototype.Instantiate();
            var card = GameObject.Instantiate<Card>(Enemy.instance.cardPrefab);

            card.transform.position = Enemy.instance.playOrigin.transform.position;
            card.SetOwner(Owner.Enemy);
            card.SetCardModel(cardModel);

            var desiredSlot = GetDesiredSlot(); 
            card.Play(desiredSlot);
        }
    }

    public int GetDesiredSlot() {
        switch(slotSelectionStrategy) {
            case SlotSelectionStrategy.PreferUnopposed:
                return FindRandomUnopposedSlot();
            case SlotSelectionStrategy.BlockHighestDamage:
                return FindBiggestAttacker();
            case SlotSelectionStrategy.KillWeakest:
                return FindLowestHealth();
            case SlotSelectionStrategy.Random:
            default:
                return -1;
        }
    }

    private int FindRandomUnopposedSlot() {
        var openSpots = new List<int>();
        for (var i = 0; i < 5; i++) {
            if(LaneManager.instance.playerSlots.IsOpenAt(i) && LaneManager.instance.enemySlots.IsOpenAt(i)) {
                openSpots.Add(i);
            }
        }
        
        if(openSpots.Count > 0) {
            return openSpots[UnityEngine.Random.Range(0, openSpots.Count)];
        } else {
            return FindBiggestAttacker();
        }
    }

    private int FindBiggestAttacker() {
        CardModel biggestCard = null;
        var biggestSlots = new List<int>();
        for(var i = 0; i < 5; i++) {
            var slot = LaneManager.instance.playerSlots.slots[i];
            if(slot.occupied && LaneManager.instance.enemySlots.IsOpenAt(i)) {
                var card = slot.objectInSlot.GetComponent<CardInLane>();
                if(card != null) {
                    if(biggestCard == null) {
                        biggestCard = card.card;
                        biggestSlots.Add(i);
                    } else if(biggestCard.damage == card.card.damage) {
                        biggestSlots.Add(i);
                    } else if(biggestCard.damage < card.card.damage) {
                        biggestCard = card.card;
                        biggestSlots.Clear();
                        biggestSlots.Add(i);
                    }
                } 
            }
        }

        if(biggestSlots.Count > 0 && biggestCard.damage > 0) {
            return biggestSlots[UnityEngine.Random.Range(0, biggestSlots.Count)];
        } else {
            return -1;
        }
    }

    private int FindLowestHealth() {
        CardInLane weakestCard = null;
        var weakestSlots = new List<int>();
        for(var i = 0; i < 5; i++) {
            var slot = LaneManager.instance.playerSlots.slots[i];
            if(slot.occupied && LaneManager.instance.enemySlots.IsOpenAt(i)) {
                var card = slot.objectInSlot.GetComponent<CardInLane>();
                if(card != null && card.cardType == Card.CardType.Monster) {
                    if(weakestCard == null) {
                        weakestCard = card;
                        weakestSlots.Add(i);
                    } else if(weakestCard.currHp == card.currHp) {
                        weakestSlots.Add(i);
                    } else if(weakestCard.currHp > card.currHp) {
                        weakestCard = card;
                        weakestSlots.Clear();
                        weakestSlots.Add(i);
                    }
                } 
            }
        }

        if(weakestSlots.Count > 0) {
            return weakestSlots[UnityEngine.Random.Range(0, weakestSlots.Count)];
        } else {
            return FindBiggestAttacker();
        }
    }
}

public class Spawn : ScriptableObject {
    public string id;
    public List<SpawnComponent> components;

    public void Execute() {
        foreach(var component in components) {
            component.Execute();
        }
    }
}

