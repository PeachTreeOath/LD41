using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public Image portrait;

    public void CreateCardImage(CardModel card)
    {
        nameText.text = card.name;
        attackText.text = card.damage + "";
        hpText.text = card.health + "";
        portrait.sprite = card.sprite;
    }
}
