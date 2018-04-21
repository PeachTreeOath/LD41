using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttackBehavior : MonoBehaviour, IGlobalAttackCooldownObject {

    public Vector3 positionDelta;

    // Use this for initialization
    void Start () {
        GlobalAttackCooldownTimer.instance.RegisterCard(this);
	}

    public void Attack()
    {
        Invoke("AttackAnimation", 0f);
    }

    private void AttackAnimation()
    {
        transform.position += positionDelta;
        Invoke("StopAnimation", .1f);
    }

    private void StopAnimation()
    {
        transform.position -= positionDelta;
    }
}
