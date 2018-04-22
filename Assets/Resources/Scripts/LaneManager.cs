using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles battling in lane and some lane utilities like placement.
public class LaneManager : Singleton<LaneManager>, IGlobalAttackCooldownObject
{
    public Card cardPrefab;
    public Slots playerSlots;
    public Slots enemySlots;

    public List<GameObject> playerSlotGameObjects;

    public Vector3 highlightStartPosition;
    public float highlightDeltaX;

    public Health playerHp;
    public Health enemyHp;

    public int startingLane = 2;

    private GameObject laneHighlight;
    private int currentLane;

    private void Start()
    {
        currentLane = startingLane;
        laneHighlight = Instantiate(ResourceLoader.instance.laneHighlight);
        SetLane(currentLane);

        GlobalAttackCooldownTimer.instance.RegisterCard(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LaneSwitch"))
        {
            NextLane();
        }

        while (playerSlots.anyOpenSlots && !playerSlots.IsOpenAt(currentLane))
        {
            NextLane();
        }
    }

    public void SetLane(int lane)
    {
        if(currentLane >= playerSlots.maxSlots)
        {
            lane = 0;
        }
        currentLane = lane;

        int startLane = currentLane;
        while(playerSlots.anyOpenSlots && !playerSlots.IsOpenAt(currentLane))
        {
            currentLane++;
            if(currentLane >= playerSlots.slots.Count)
            {
                currentLane = 0;
            }
            //We looped the lanes, not going to find one
            if(currentLane == startLane)
            {
                //TODO: Make the highlight go invisible when this happens
                break;
            }
        }

        laneHighlight.transform.position = playerSlotGameObjects[currentLane].transform.position;
    }

    public void NextLane()
    {
        SetLane(++currentLane);
    }

    public int GetLane()
    {
        return currentLane;
    }

    public ObjectSlot ClaimEnemySlot(GameObject go, int desiredIndex = -1, bool failIfNotOpen = false)
    {
        if (!enemySlots.anyOpenSlots) return null;

        if (desiredIndex < 0)
        {
            desiredIndex = UnityEngine.Random.Range(0, enemySlots.maxSlots);
        }

        ObjectSlot slot = null;
        if (failIfNotOpen)
        {
            slot = enemySlots.ClaimSlot(go, desiredIndex);
        }
        else
        {
            for (int i = 0; i < enemySlots.maxSlots; i++)
            {
                var index = (desiredIndex + i) % enemySlots.maxSlots;
                slot = enemySlots.ClaimSlot(go, index);
                if (slot)
                {
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
                if (card.cardType == Card.CardType.Spell)
                {
                    SpellInLane spell = playerCard.GetComponent<SpellInLane>();
                    spell.CountdownSpell();
                    if (spell.timeToCast == 0)
                        OnRemove(spell);
                }
                else
                {
                    enemyHp.DealDamage(card.GetAttackDamage());
                }
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
                if (playerHp <= 0)
                {
                    OnRemove(pCard);
                }
                if (enemyHp <= 0)
                {
                    OnRemove(eCard);
                }
            }
        }
    }

    private void OnRemove(CardInLane cardInLane)
    {
        var card = GameObject.Instantiate<Card>(cardPrefab);
        card.SetOwner(cardInLane.owner);
        card.SetCardModel(cardInLane.card);

        cardInLane.slot.Release();

        card.transform.position = cardInLane.transform.position;
        card.SetInLane(); //TODO add the slot here?
        card.Discard();

        //TODO show death effect or something?

        Destroy(cardInLane.gameObject);
    }
}
