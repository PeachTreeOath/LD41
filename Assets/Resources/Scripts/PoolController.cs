using System;
using UnityEngine;
using System.Collections.Generic;

public class PoolController : MonoBehaviour, IGlobalTimedObject {

    private List<LootTableRow> drawTable;

    private Pool pool;
    private Timer drawTimer;
    private AiAnimationController drawRateController;

    public String startingAnimation;

    void Start() {
        pool = GetComponent<Pool>();
        drawTimer = new Timer();

        drawRateController = GetComponent<AiAnimationController>();
        drawRateController.animValueEvent.AddListener(OnValueUpdate);
        //drawRateController.animEventEvent.AddListener(OnAnimEvent);
    }

    public void ManualUpdate(float deltaTime) {
        if( drawTimer.Update(deltaTime) ) {
            pool.DrawToPool();
        }
    }

    private void OnValueUpdate(float value) {
        drawTimer.target = value;
    }

    private void OnAnimEvent(string eventText) {
        //drawRateController.InterpretCommand(eventText); 
    }
}
