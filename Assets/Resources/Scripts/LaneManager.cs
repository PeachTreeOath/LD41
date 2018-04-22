using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles battling in lane and some lane utilities like placement.
public class LaneManager : Singleton<LaneManager>, IGlobalAttackCooldownObject
{
    public Slots playerSlots;
    public Slots enemySlots;

    public Vector3 highlightStartPosition;
    public float highlightDeltaX;

    public Health playerHp;
    public Health enemyHp;

    private GameObject laneHighlight;
    private int currentLane = 2;

    private void Start()
    {
        laneHighlight = Instantiate(ResourceLoader.instance.laneHighlight);
        laneHighlight.transform.position = highlightStartPosition;

        GlobalAttackCooldownTimer.instance.RegisterCard(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LaneSwitch"))
        {
            currentLane++;
            if (currentLane > 4)
                currentLane = 0;

            float xPos = highlightStartPosition.x + ((currentLane - 2) * highlightDeltaX);
            laneHighlight.transform.position = new Vector3(xPos, laneHighlight.transform.position.y, laneHighlight.transform.position.z);
        }
    }

    public int GetLane()
    {
        return currentLane;
    }

    public ObjectSlot ClaimEnemySlot(GameObject go, int desiredIndex=-1, bool failIfNotOpen=false) {
        if (!enemySlots.anyOpenSlots) return null;

        if(desiredIndex < 0) {
            desiredIndex = UnityEngine.Random.Range(0, enemySlots.maxSlots);
        }

        ObjectSlot slot = null;
        if(failIfNotOpen) {
            slot = enemySlots.ClaimSlot(go, desiredIndex); 
        } else {
            for(int i = 0; i < enemySlots.maxSlots; i++) {
                var index = (desiredIndex + i) % enemySlots.maxSlots;
                slot = enemySlots.ClaimSlot(go, index);
                if(slot) {
                    break;
                }
            }
        }

        return slot;
    }

    public ObjectSlot ClaimPlayerSlot(GameObject go)
    {
        if (!playerSlots.anyOpenSlots) return null;

        for (int i = 0; i < playerSlots.maxSlots; i++)
        {
            var index = (currentLane + i) % playerSlots.maxSlots;
            var slot = playerSlots.ClaimSlot(go, index);
            if (slot)
            {
                return slot;
            }
        }

        return null;
    }

    public void Attack()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject playerCard = playerSlots.slots[i].objectInSlot;
            GameObject enemyCard = enemySlots.slots[i].objectInSlot;

            if (playerCard == null && enemyCard == null)
            {
                continue;
            }
            else if (playerCard != null && enemyCard == null)
            {
                CardInLane card = playerCard.GetComponent<CardInLane>();
                enemyHp.DealDamage(card.GetAttackDamage());
            }
            else if (playerCard == null && enemyCard != null)
            {
                CardInLane card = enemyCard.GetComponent<CardInLane>();
                playerHp.DealDamage(card.GetAttackDamage());
            }
            else if (playerCard != null && enemyCard != null)
            {
                CardInLane pCard = playerCard.GetComponent<CardInLane>();
                CardInLane eCard = enemyCard.GetComponent<CardInLane>();
                int playerHp = pCard.TakeDamage(eCard.GetAttackDamage());
                int enemyHp = eCard.TakeDamage(pCard.GetAttackDamage());
                if (playerHp <= 0) pCard.RemoveFromPlay();
                if (enemyHp <= 0) eCard.RemoveFromPlay();

            }
        }
    }
}
