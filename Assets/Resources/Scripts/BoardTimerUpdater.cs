using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardTimerUpdater : MonoBehaviour
{
    private GlobalAttackCooldownTimer timer;
    private TextMeshProUGUI text;

    // Use this for initialization
    void Start()
    {
        timer = GlobalAttackCooldownTimer.instance;
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (int)(timer.tickTime - timer.currentTime) + "";
    }
}
