using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public Color redColor;
    public Color blueColor;
    public Color grayColor;

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
                topTriangle.GetComponent<Image>().color = redColor;
                bottomTriangle.GetComponent<Image>().color = grayColor;
                bottomTriangleIcon.enabled = true;
                bottomTriangleIcon.GetComponent<RectTransform>().localScale *= .9f;
                bottomTriangleIcon.sprite = ResourceLoader.instance.attackIcon;
            }
            else
            {
                topTriangleText.text = "";
                bottomTriangle.GetComponent<Image>().color = grayColor;

                int num = card.cardsToDraw + card.casterHpToHeal;
                bottomTriangleText.text = num + "";
                topTriangleIcon.enabled = true;
                topTriangleIcon.GetComponent<RectTransform>().localScale *= .5f;
                bottomTriangle.GetComponent<Image>().color = blueColor;
                topTriangle.GetComponent<Image>().color = grayColor;
                if (card.cardsToDraw > 0)
                    topTriangleIcon.sprite = ResourceLoader.instance.drawIcon;
                else if(card.casterHpToHeal > 0)
                    topTriangleIcon.sprite = ResourceLoader.instance.healIcon;
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
