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

    public int[] playerDamageArray = new int[5]; // All the dmg the player row is about to take
    public int[] enemyDamageArray = new int[5]; // All the dmg the enemy row is about to take

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
        if (currentLane >= playerSlots.maxSlots)
        {
            lane = 0;
        }
        currentLane = lane;

        int startLane = currentLane;
        while (playerSlots.anyOpenSlots && !playerSlots.IsOpenAt(currentLane))
        {
            currentLane++;
            if (currentLane >= playerSlots.slots.Count)
            {
                currentLane = 0;
            }
            //We looped the lanes, not going to find one
            if (currentLane == startLane)
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
        // Reset dmg queues
        for (int i = 0; i < 5; i++)
        {
            playerDamageArray[i] = 0;
            enemyDamageArray[i] = 0;
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject playerCard = playerSlots.slots[i].objectInSlot;
            GameObject enemyCard = enemySlots.slots[i].objectInSlot;

            if (playerCard == null && enemyCard == null)
            {
                continue;
            }
            else if (playerCard != null)
            {
                CardInLane card = playerCard.GetComponent<CardInLane>();
                if (card.cardType == Card.CardType.Spell)
                {
                    SpellInLane spell = playerCard.GetComponent<SpellInLane>();
                    spell.CountdownSpell(i, true);
                    if (spell.timeToCast == 0)
                        OnRemove(spell);
                }
                else
                {
                    enemyDamageArray[i] += card.GetAttackDamage();
                }
            }
            else if (enemyCard != null)
            {
                CardInLane card = enemyCard.GetComponent<CardInLane>();
                if (card.cardType == Card.CardType.Spell)
                {
                    SpellInLane spell = enemyCard.GetComponent<SpellInLane>();
                    spell.CountdownSpell(i, false);
                    if (spell.timeToCast == 0)
                        OnRemove(spell);
                }
                else
                {
                    playerDamageArray[i] += card.GetAttackDamage();
                }
            }
        }
        // Resolve damage queues
        for (int i = 0; i < 5; i++)
        {
            AttackSlot(i, enemySlots, enemyDamageArray[i], enemyHp);
            AttackSlot(i, playerSlots, playerDamageArray[i], playerHp);
        }
    }

    public void AttackSlot(int index, Slots attackedSlots, int damage, Health targetHealth)
    {
        GameObject card = attackedSlots.slots[index].objectInSlot;
        if (card == null) // No defender, hit face
        {
            targetHealth.DealDamage(damage);
        }
        else // Make defender take damage
        {
            CardInLane defender = card.GetComponent<CardInLane>();
            if (defender.cardType == Card.CardType.Monster)
            {
                int defenderHp = defender.TakeDamage(damage);
                if (defenderHp <= 0)
                {
                    OnRemove(defender);
                }
            }
            else
            {
                // Attack bypasses spells, take face dmg
                targetHealth.DealDamage(damage);
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
