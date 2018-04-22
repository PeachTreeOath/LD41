﻿using System;
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

    private GameObject laneHighlight;
    private int currentLane = 2;

    private void Start()
    {
        laneHighlight = Instantiate(ResourceLoader.instance.laneHighlight);
        laneHighlight.transform.position = highlightStartPosition;
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
            GameObject playerCard = playerSlots.slots[0].gameObject;
            GameObject enemyCard = playerSlots.slots[0].gameObject;

            if (playerCard == null && enemyCard == null)
            {
                continue;
            }
            else if (playerCard != null && enemyCard == null)
            {
                CardInLane card = playerCard.GetComponent<CardInLane>();
                //TODO: Attack face
            }
            else if (playerCard == null && enemyCard != null)
            {
                CardInLane card = enemyCard.GetComponent<CardInLane>();
                //TODO: Attack player face
            }
            else if (playerCard != null && enemyCard != null)
            {
                CardInLane pCard = playerCard.GetComponent<CardInLane>();
                CardInLane eCard = enemyCard.GetComponent<CardInLane>();
                int playerDmg = pCard.TakeDamage(eCard.GetAttackDamage());
                int enemyDmg = eCard.TakeDamage(pCard.GetAttackDamage());
            }
        }
    }
}
