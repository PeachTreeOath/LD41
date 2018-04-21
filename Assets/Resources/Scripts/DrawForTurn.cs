using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawForTurn : MonoBehaviour, IGlobalAttackCooldownObject {

    public void Start() {
        GlobalAttackCooldownTimer.instance.RegisterCard(this);
    }

    public void Attack() {
        if( Hand.instance.canDrawCards ) {
            Hand.instance.DrawFromDeck();
        }
    }

    public void OnDestroy() {
        //GlobalAttackCooldownTimer.instance.UnregisterCard(this);
    }
}
