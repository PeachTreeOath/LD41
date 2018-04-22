using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Health : MonoBehaviour {

    public int max = 20;
    public int start = 20;

    public int current { get; private set; }

    void Start()
    {
        Assert.IsTrue(start <= max);
        current = start;
    }

    public void DealDamage(int amount)
    {
        current -= amount;

        if(current <= 0)
        {
            //The GameManager will check in with us later to do a Game Over
            current = 0;
        }
    }

    public void HealDamage(int amount)
    {
        current += amount;

        if(current > max)
        {
            current = max;
        }
    }
}
