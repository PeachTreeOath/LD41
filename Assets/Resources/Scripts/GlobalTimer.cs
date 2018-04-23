using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls time for anything that needs to be paused outside the core update loop.
public class GlobalTimer : Singleton<GlobalTimer>
{
    public float timeScale = 1f;
    private List<IGlobalTimedObject> timedObjectList = new List<IGlobalTimedObject>();
    private bool isPaused;

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            var dt = Time.deltaTime * timeScale;
            foreach (IGlobalTimedObject obj in timedObjectList)
            {
                obj.ManualUpdate(dt);
            }
        }
    }

    public void RegisterObject(IGlobalTimedObject timedObject)
    {
        timedObjectList.Add(timedObject);
    }

    public void PauseTimer(bool pause)
    {
        isPaused = pause;
    }

}
