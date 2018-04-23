using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttackBehavior : MonoBehaviour, IGlobalAttackCooldownObject
{

    public Vector3 positionDelta;

    private float attackTime;
    private bool isAttacking;
    private float attackDuration = .2f;

    // Use this for initialization
    void Start()
    {
        GlobalAttackCooldownTimer.instance.RegisterCard(this);
    }

    private void Update()
    {
        if (isAttacking)
        {
            attackTime += Time.deltaTime;
            if (attackTime > attackDuration)
            {
                StopAnimation();
            }
        }
    }

    public void Attack()
    {
        isAttacking = true;
        transform.position += positionDelta;
    }

    private void StopAnimation()
    {
        transform.position -= positionDelta;
        isAttacking = false;
        attackTime = 0;
    }

    void OnDestroy()
    {
        if (GlobalAttackCooldownTimer.instance != null)
            GlobalAttackCooldownTimer.instance.DeregisterCard(this);
    }
}
