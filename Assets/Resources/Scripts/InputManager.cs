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
	void Update () {

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
        
        foreach(Card card in eligibleCards)
        {
            print("#cards: " + eligibleCards.Count);
            string cardName = card.cardModel.name;
            TextMeshProUGUI cardTextField = card.cardView.nameText;
            if (inputText.Length > 0 && cardTextField.text.StartsWith(inputText))
            {
                string highlightedLetters = inputText;
                string restOfWord = cardTextField.text.Substring(inputText.Length);
                cardTextField.text = "<color=red>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";
                print("match! inputString: " + inputText);
            }
            
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
