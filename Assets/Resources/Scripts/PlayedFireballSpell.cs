using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayedFireballSpell : SpellInLane
{
    protected override void CastSpell(int slotIndex, bool isAttackingEnemy)
    {
        if (isAttackingEnemy)
            LaneManager.instance.enemyDamageArray[slotIndex] += card.damage;
        else
            LaneManager.instance.playerDamageArray[slotIndex] += card.damage;
    }
}
