using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInLaneView : MonoBehaviour {

    public TextMeshProUGUI attackDefenseText;
    public Image portrait;

    public void CreateCardImage(CardModel card)
    {
        attackDefenseText.text = card.damage + "/" + card.health;
        portrait.sprite = card.sprite;
    }
}
