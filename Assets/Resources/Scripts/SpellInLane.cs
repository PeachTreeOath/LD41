using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellInLane : CardInLane
{
    protected abstract void CastSpell(int slotIndex, bool isAttackingEnemy);

    public void CountdownSpell(int slotIndex, bool isAttackingEnemy)
    {
        timeToCast--;
        //TODO: Reflect in UI
        if (timeToCast <= 0)
        {
            CastSpell(slotIndex, isAttackingEnemy);
        }
    }
}
