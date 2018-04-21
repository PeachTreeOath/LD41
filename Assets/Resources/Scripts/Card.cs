using UnityEngine;
using System;

[Serializable]
public class Card {
    public CardPrototype prototype;
    public String name;
    public Sprite sprite;
    public int health;
    public int damage;

    public Card(CardPrototype prototype) {
        this.prototype = prototype;
        this.name = prototype.cardName;
        this.health = prototype.health;
        this.damage = prototype.damage;
        this.sprite = prototype.sprite;
    }
}


