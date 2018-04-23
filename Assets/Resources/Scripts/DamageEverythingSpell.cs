using UnityEngine;
using System.Collections;

public class DamageEverythingSpell : SpellInLane {

    public bool friendlyFire = true;
    public bool destroySpells = false;

    protected override void CastSpell(int slotIndex, bool isAttackingEnemy) {
        if(friendlyFire || owner == Owner.Player)
            DamageEverything(LaneManager.instance.enemyDamageArray, LaneManager.instance.enemySlots);
        if (friendlyFire || owner == Owner.Enemy)
            DamageEverything(LaneManager.instance.playerDamageArray, LaneManager.instance.playerSlots);
    }

    private void DamageEverything(int[] damage, Slots slots) {
        for (var i = 0; i < 5; i++) {
            var slot = slots.slots[i];
            if(slot.occupied) {
                var card = slot.objectInSlot.GetComponent<CardInLane>();
                if(card != null) {
                    if (this.card.damage > 0 && card.cardType == Card.CardType.Monster) {
                        damage[i] += this.card.damage;
                    } else if (card.cardType == Card.CardType.Spell && destroySpells) {
                        //TODO hacky-- spells resolve left to right, should really occur in first pass
                        LaneManager.instance.Remove(card);
                    }
                }
            }
        }
    }

}
