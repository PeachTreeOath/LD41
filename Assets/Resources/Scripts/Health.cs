using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class Health : MonoBehaviour {

    public int max = 20;
    public int start = 20;
    public TextMeshProUGUI healthText;
    public Owner owner;

    public int current { get; private set; }

    void Awake()
    {
        Assert.IsTrue(start <= max);
        current = start;
    }

    void Update() {
        healthText.text = current + " hp";
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
