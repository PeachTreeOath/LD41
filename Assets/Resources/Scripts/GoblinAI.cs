using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

[Serializable]
public class Threshold {
    public bool tripped = false;
    public int healthTrigger = 100;
    public string animToPlay = "WakeUp";

    public bool couldTrip {
        get {
            return !tripped && Enemy.instance.health.current < healthTrigger;
        }
    }
}

[RequireComponent(typeof(AiSpawnController))]
public class GoblinAI : MonoBehaviour, IGlobalTimedObject {

    //TODO do we need curves for more than one value?
    //TODO make this not a behavior?
    private AiAnimationController spawnRateController;
    private AiSpawnController spawnController;
    private Timer spawnTimer;

    public List<Threshold> thresholdTriggers;

    public void ManualUpdate(float deltaTime) {
        if (spawnTimer.Update(deltaTime)) spawnController.Spawn();
        CheckThresholds();
    }

    void Start() {
        spawnController = GetComponent<AiSpawnController>();

        spawnRateController = GetComponent<AiAnimationController>();
        spawnRateController.animValueEvent.AddListener(UpdateSpawnRate);
        spawnRateController.animEventEvent.AddListener(OnAnimEvent);

        GlobalTimer.instance.RegisterObject(this);

        if (spawnTimer == null) spawnTimer = new Timer();
    }

    private void UpdateSpawnRate(float value) {
        this.spawnTimer.target = value;
    }

    private void OnAnimEvent(string eventText) {
        var match = Regex.Match(eventText, @"^([^:]+):\s+(.*$)"); 
        if(match.Success) {
            var command = match.Groups[1].Value;
            var argument = match.Groups[2].Value;

            spawnController.InterpretDirective(command, argument);
        } else {
            Debug.LogWarningFormat("Ignoring unknown event '{0}'", eventText);
        }
    }

    private bool InterpretCommand(string command, string argument) {
        if(command.ToLower() == "") {

        }

        return false;
    }

    private void CheckThresholds() {
        Threshold toTrigger = null;
        foreach(var threshold in thresholdTriggers) {
            if(threshold.couldTrip) {
                threshold.tripped = true;
                if(toTrigger == null || toTrigger.healthTrigger > threshold.healthTrigger) {
                    toTrigger = threshold;
                }
            }
        }

        if(toTrigger != null) {
            spawnRateController.PlayAnimation(toTrigger.animToPlay);
        }
    }
}

