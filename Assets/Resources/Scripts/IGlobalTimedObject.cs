using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GlobalTimer calls these
public interface IGlobalTimedObject
{

    // Call this instead of relying on Update()
    void ManualUpdate(float deltaTime);

}
