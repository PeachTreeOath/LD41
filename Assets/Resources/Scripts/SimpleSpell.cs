using UnityEngine;
using System.Collections;

public class SimpleSpell : SpellInLane {

    //TODO isAttackingEnemy is superfluous
    protected override void CastSpell(int slotIndex, bool isAttackingEnemy) {

        if(card.damage > 0) DealDamage(slotIndex);
        if (card.cardsToDraw > 0) DrawCards();
        if (card.casterHpToHeal > 0) HealCaster();
    }

    private void DrawCards() {
        if (owner == Owner.Player) {
            for (var i = 0; i < card.cardsToDraw; i++) {
                Deck.instance.Draw();
            }
        }
    }

    private void DealDamage(int slotIndex) {
        if (owner == Owner.Player)
            LaneManager.instance.enemyDamageArray[slotIndex] += card.damage;
        else
            LaneManager.instance.playerDamageArray[slotIndex] += card.damage;
    }

    private void HealCaster() {
        if (owner == Owner.Player)
            Player.instance.health.HealDamage(card.casterHpToHeal);
        else
            Enemy.instance.health.HealDamage(card.casterHpToHeal);
    }
}
