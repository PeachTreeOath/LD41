using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour {

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;

    private string inputText = "";

    // Update is called once per frame
    void Update()
    {

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
        if (inputText.Length > 0 && nameText.text.StartsWith(inputText))
        {
            string highlightedLetters = inputText;
            string restOfWord = nameText.text.Substring(inputText.Length);

            nameText.text = "<color=red>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";
            print("match!");
        }
    }

    public void CreateCardImage(CardModel card)
    {
        nameText.text = card.name;
        attackText.text = card.damage + "";
        healthText.text = card.health + "";
    }
}
