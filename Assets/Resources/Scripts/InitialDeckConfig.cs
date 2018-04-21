﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitialDeckConfig : MonoBehaviour {

    public List<CardPrototype> cardsToStart;

    void Start() {
        if(cardsToStart == null) {
            cardsToStart = new List<CardPrototype>();
        }

        for(var i = cardsToStart.Count - 1; i >= 0; i--) {
            var card = cardsToStart[i];
            Deck.instance.PutOnTop( card.Instantiate() );
        }
    }
}