using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class SpawnComponent {
    public CardPrototype prototype;

    public void Execute() {
        if (!LaneManager.instance.enemySlots.anyOpenSlots) return;

        var cardModel = prototype.Instantiate();
        var card = GameObject.Instantiate<Card>(Enemy.instance.cardPrefab);

        card.transform.position = Enemy.instance.playOrigin.transform.position;
        card.SetOwner(Owner.Enemy);
        card.SetCardModel(cardModel);

        var desiredSlot = -1; //TODO make this work
        card.Play();
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

