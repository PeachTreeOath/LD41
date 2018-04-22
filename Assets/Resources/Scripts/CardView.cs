using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour {

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;



    public void CreateCardImage(CardModel card)
    {
        nameText.text = card.name;
        attackText.text = card.damage + "";
        healthText.text = card.health + "";
    }
}
