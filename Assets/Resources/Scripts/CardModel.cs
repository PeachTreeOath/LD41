﻿using UnityEngine;
using System;

[Serializable]
public class CardModel {
    public CardPrototype prototype;
    public String name;
    public Sprite sprite;
    public int health;
    public int damage;
    public int timeToCast;
    public int cardsToDraw;
    public int casterHpToHeal;

    public CardModel(CardPrototype prototype) {
        this.prototype = prototype;
        this.name = prototype.cardName;
        this.health = prototype.health;
        this.damage = prototype.damage;
        this.sprite = prototype.sprite;
        this.timeToCast = prototype.timeToCast;
        this.cardsToDraw = prototype.cardsToDraw;
        this.casterHpToHeal = prototype.casterHpToHeal;
    }


}


