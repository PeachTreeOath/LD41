using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInLaneView : MonoBehaviour {

    public Color redColor;
    public Color blueColor;
    public Color grayColor;

    // Monster stuff
    public GameObject botAttackObj;
    public GameObject botHpObj;
    public TextMeshProUGUI botAttackText;
    public TextMeshProUGUI botHpText;
    public GameObject topAttackObj;
    public GameObject topHpObj;
    public TextMeshProUGUI topAttackText;
    public TextMeshProUGUI topHText;

    // Spell stuff
    public GameObject topTriangle;
    public GameObject bottomTriangle;
    public TextMeshProUGUI topTriangleText;
    public TextMeshProUGUI bottomTriangleText;
    public Image topTriangleIcon;
    public Image bottomTriangleIcon;

    public GameObject castTimeObj;
    public TextMeshProUGUI castTimeText;

    public Image portrait;

    public void CreateCardImage(CardModel card)
    {
        portrait.sprite = card.sprite;
    }
}
