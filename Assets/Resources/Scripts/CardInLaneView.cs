using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInLaneView : MonoBehaviour
{

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
    public TextMeshProUGUI topHpText;

    // Spell stuff
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

    public void CreateCardImage(CardModel card, Owner owner, int currHp)
    {
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
                else if (card.casterHpToHeal > 0)
                    topTriangleIcon.sprite = ResourceLoader.instance.healIcon;
            }
        }
        else // monster
        {
            if (owner == Owner.Player)
            {
                botAttackText.text = card.damage + "";
                botHpText.text = currHp + "";
            }
            else
            {
                botAttackObj.SetActive(false);
                botHpObj.SetActive(false);
                topAttackObj.SetActive(true);
                topHpObj.SetActive(true);
                topAttackText.text = card.damage + "";
                topHpText.text = currHp + "";
            }
        }
        portrait.sprite = card.sprite;
    }
}
