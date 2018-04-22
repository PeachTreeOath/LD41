using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GoblinAI : MonoBehaviour, IGlobalTimedObject {

    //TODO do we need curves for more than one value?
    //TODO make this not a behavior?
    private AiAnimationController spawnRateController;

    public Timer spawnTimer;
    public Spawn minionSpawn;

    public void ManualUpdate(float deltaTime) {
        if (spawnTimer.Update(deltaTime)) minionSpawn.Execute();
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
}
