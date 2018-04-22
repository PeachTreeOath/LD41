using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickDownHealth : MonoBehaviour, IGlobalAttackCooldownObject
{
    public Health health;
    public int damagePerTick = 5;

	// Use this for initialization
	void Start () {
        GlobalAttackCooldownTimer.instance.RegisterCard(this);
    }

    public void Attack()
    {
        health.DealDamage(damagePerTick);
    }
}
