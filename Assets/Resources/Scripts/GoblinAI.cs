using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoblinAI : MonoBehaviour, IGlobalTimedObject {

    //TODO do we need curves for more than one value?
    //TODO make this not a behavior?
    private AiAnimationController spawnRateController;

    public Timer spawnTimer;
    public CardPrototype minionSpawn;

    public void ManualUpdate(float deltaTime) {
        if (spawnTimer.Update(deltaTime)) Spawn(minionSpawn);
    }

    void Start() {
        spawnRateController = GetComponent<AiAnimationController>();
        spawnRateController.animValueEvent.AddListener(UpdateSpawnRate);
        spawnRateController.PlayAnimation("WarmUp");

        GlobalTimer.instance.RegisterObject(this);

        if (spawnTimer == null) spawnTimer = new Timer();
    }

    private void UpdateSpawnRate(float value) {
        this.spawnTimer.target = value;
    }

    private void Spawn(CardPrototype prototype) {
        if (!LaneManager.instance.enemySlots.anyOpenSlots) return;

        var cardModel = prototype.Instantiate();
        var card = GameObject.Instantiate<Card>(Enemy.instance.cardPrefab);

        card.transform.position = Enemy.instance.playOrigin.transform.position;
        card.SetOwner(Owner.Enemy);
        card.SetCardModel(cardModel);

        var desiredSlot = DefaultFindSlot();
        card.Play();
    }

    private int DefaultFindSlot() {
        return -1;
    }
}
