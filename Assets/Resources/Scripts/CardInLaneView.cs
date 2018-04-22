using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardInLaneView : MonoBehaviour {

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;

    private string inputText = "";

    public void CreateCardImage(CardInLane card)
    {
        nameText.text = card.card.name;
        attackText.text = card.card.damage + "";
        healthText.text = card.currHp + "";
    }
}
