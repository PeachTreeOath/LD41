using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    private Color redColor;
    private Color blueColor;
    private Color grayColor;

    public TextMeshProUGUI nameText;
    public GameObject botAttackObj;
    public GameObject botHpObj;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public GameObject topTriangle;
    public GameObject bottomTriangle;
    public TextMeshProUGUI topTriangleText;
    public TextMeshProUGUI bottomTriangleText;
    public Image topTriangleIcon;
    public Image bottomTriangleIcon;

    public GameObject castTimeObj;
    public TextMeshProUGUI castTimeText;

    public Image border;
    public Image portrait;

    private void Start()
    {
        ColorUtility.TryParseHtmlString("EB5B57FF", out redColor);
        ColorUtility.TryParseHtmlString("5AC9F2FF", out blueColor);
        ColorUtility.TryParseHtmlString("363636FF", out grayColor);
    }

    public void CreateCardImage(CardModel card)
    {
        nameText.text = card.name;
        if (card.timeToCast >= 0) // spell
        {
            border.enabled = false;
            botAttackObj.SetActive(false);
            botHpObj.SetActive(false);
            topTriangle.SetActive(true);
            bottomTriangle.SetActive(true);
            castTimeObj.SetActive(true);

            castTimeText.text = card.timeToCast + "";

            // Attack spell
            if (card.damage > 0)
            {
                topTriangleText.text = card.damage + "";
                bottomTriangleText.text = "";
                bottomTriangle.GetComponent<Image>().color = grayColor;
            }
        }
        else // monster
        {
            attackText.text = card.damage + "";
            hpText.text = card.health + "";
        }
        portrait.sprite = card.sprite;

    }
}
