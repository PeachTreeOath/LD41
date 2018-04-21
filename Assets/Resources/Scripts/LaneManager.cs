using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : Singleton<LaneManager>
{

    public Vector3 highlightStartPosition;
    public float highlightDeltaX;

    private GameObject laneHighlight;
    private int currentLane = 2;

    private void Start()
    {
        laneHighlight = Instantiate(ResourceLoader.instance.laneHighlight);
        laneHighlight.transform.position = highlightStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LaneSwitch"))
        {
            currentLane++;
            if (currentLane > 4)
                currentLane = 0;

            float xPos = highlightStartPosition.x + ((currentLane - 2) * highlightDeltaX);
            laneHighlight.transform.position = new Vector3(xPos, laneHighlight.transform.position.y, laneHighlight.transform.position.z);
        }
    }

    public int GetLane()
    {
        return currentLane;
    }
}
