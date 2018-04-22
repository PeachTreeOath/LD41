using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cards register to this to attack at the same time.
public class GlobalAttackCooldownTimer : Singleton<GlobalAttackCooldownTimer>, IGlobalTimedObject
{
    private List<IGlobalAttackCooldownObject> cardList = new List<IGlobalAttackCooldownObject>();
    private float tickTime;
    private float currentTime;

    void Start()
    {
        GlobalTimer.instance.RegisterObject(this);
    }

    public void RegisterCard(IGlobalAttackCooldownObject card)
    {
        cardList.Add(card);
    }

    public void DeregisterCard(IGlobalAttackCooldownObject card)
    {
        cardList.Remove(card);
    }

    public void ManualUpdate(float deltaTime)
    {
        currentTime += deltaTime;
        if (currentTime > tickTime)
        {
            currentTime -= tickTime;

            // Trigger all registered cards
            foreach(IGlobalAttackCooldownObject card in cardList)
            {
                card.Attack();
            }
        }
    }

    public void SetTickTime(float tick)
    {
        tickTime = tick;
    }
}
