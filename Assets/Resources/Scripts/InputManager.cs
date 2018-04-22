using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{

    //This is a test case
    //public string cardText = "New Text";

    //private List<IInputListener> listeners = new List<IInputListener>();

    //private List<ZoneEvent> zoneEvents = new List<ZoneEvent>();
    private string inputText = "";
    //private TextMeshProUGUI textField;

    private List<Card> eligibleCards = new List<Card>();
    
	// Use this for initialization
	void Start () {
        Hand.instance.handZoneEvent.AddListener(CardHasEnterredOrExittedHand);
        Pool.instance.poolZoneEvent.AddListener(CardHasEnterredOrExittedHand);

    }

    // Update is called once per frame
    void Update() {

        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                if (inputText.Length != 0)
                {
                    inputText = inputText.Substring(0, inputText.Length - 1);
                }
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                //print("User entered their name: " + inputText);
            }
            else
            {
                inputText += c;
            }
        }


        List<Card> matchedCards = new List<Card>();
        bool needToClearBuffer = false;
        foreach (Card card in eligibleCards)
        {
            string cardName = card.cardModel.name;
            TextMeshProUGUI cardTextField = card.cardView.nameText;
            if (cardName.Equals(inputText))
            {
                if (card.state == Card.State.InHand && LaneManager.instance.playerSlots.AreAllSlotsBlocked())
                {
                    //Case where the lane is blocked and we are trying to complete a card.
                    //We will just clear the buffer here and not attempt to remove the card.
                    needToClearBuffer = true;
                }
                else
                {
                    matchedCards.Add(card);
                }

            } else if (inputText.Length > 0 && cardName.StartsWith(inputText))
            {
                string highlightedLetters = inputText;
                string restOfWord = cardName.Substring(inputText.Length);
                cardTextField.text = "<color=blue>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";
            }
            else if (inputText.Length > 0)
            {
                //User has typed something but it doesn't match anything, we need to force a backspace
                inputText = inputText.Substring(0, inputText.Length - 1);
            }

        }

        if (needToClearBuffer)
        {
            inputText = "";
            foreach(Card card in eligibleCards)
            {
                //Now reset all highlighting
                card.cardView.nameText.text = card.cardModel.name;
            }
        }

        //Doing this because we couldn't remove cards while going through a foreach loop.
        Card[] cardArr = matchedCards.ToArray();
        for (int i = 0; i < cardArr.Length; i++)
        {
            cardArr[i].MatchedWord();
        }
        if (cardArr.Length > 0)
        {
            inputText = "";
        }


    }


    public void CardHasEnterredOrExittedHand(string enterredOrExitted, Card card)
    {
        if (enterredOrExitted.Equals(ZoneEvent.ENTERED))
        {
            eligibleCards.Add(card);
        }
        else
        {
            eligibleCards.Remove(card);
        }
    }


}
