using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackDefenseText;
    public Image portrait;

    public void CreateCardImage(CardModel card)
    {
        nameText.text = card.name;
        attackDefenseText.text = card.damage + "/" + card.health;
        portrait.sprite = card.sprite;
    }
}
