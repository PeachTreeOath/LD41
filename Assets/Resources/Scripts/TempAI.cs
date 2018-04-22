using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAI : MonoBehaviour {

    public Card cardPrefab;
    public CardPrototype cardPrototype;
    public GameObject playOrigin;

    public void PlayCard() {
        if (!LaneManager.instance.enemySlots.anyOpenSlots) return;

        var cardModel = cardPrototype.Instantiate();
        var card = GameObject.Instantiate<Card>(cardPrefab);

        card.transform.position = playOrigin.transform.position;
        card.SetOwner(Owner.Enemy);
        card.SetCardModel(cardModel);

        card.Play();
    }
}
