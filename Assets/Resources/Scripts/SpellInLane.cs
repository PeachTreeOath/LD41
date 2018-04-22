using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellInLane : CardInLane
{

    public void CountdownSpell()
    {
        timeToCast--;
        //TODO: Reflect in UI
        if(timeToCast <= 0)
        {
            CastSpell();
        }
    }

    protected abstract void CastSpell();
}
