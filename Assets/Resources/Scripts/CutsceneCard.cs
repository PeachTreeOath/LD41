using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutsceneCard : MonoBehaviour {

    public string typableText;
    public TextMeshProUGUI cardTextField;
    public string nextScene;

    public string inputText;

    public void Start()
    {
        TextMeshProUGUI[] textFields = GetComponentsInChildren<TextMeshProUGUI>();
        foreach(TextMeshProUGUI textField in textFields)
        {
            if (textField.name.Equals("TypableText")){
                cardTextField = textField;
            }
        }
    }

    public void InputText(string inputString)
    {
        foreach (char c in inputString)
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
                //enter clears what they typed
                inputText = "";
            }
            else
            {
                inputText += c;
            }
        }
        
        if (typableText.Equals(inputText, StringComparison.CurrentCultureIgnoreCase))
        {
            cardTextField.text = "<color=blue>" + typableText + "</color>";
            //We matched the word, move progress to the next scene.
            SceneTransitionManager.instance.loadScene(nextScene);
        }
        else if (inputText.Length > 0 && typableText.StartsWith(inputText, StringComparison.CurrentCultureIgnoreCase))
        {
            string highlightedLetters = typableText.Substring(0, inputText.Length);
            string restOfWord = typableText.Substring(inputText.Length);
            cardTextField.text = "<color=blue>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";

            //float textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
            //float bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;

            //cardTextField.transform.parent.GetComponent<Image>().color = bannerActiveColor;
            /*
            int frontNumToRemove = 0;
            int backNumToRemove = 0;
            
            //This makes it so we always show 10 highlighted characters then the rest are not highlighted
            while (textWidth > bannerWidth && shortCardName.Substring(0, inputText.Length - frontNumToRemove).Length > 10)
            {
                //Remove a character from the front
                frontNumToRemove++;
                shortCardName = shortCardName.Substring(1);

                //Recalculate size
                highlightedLetters = shortCardName.Substring(0, inputText.Length - frontNumToRemove);
                restOfWord = shortCardName.Substring(inputText.Length - frontNumToRemove);
                cardTextField.text = "<color=#3498DB>" + highlightedLetters + "</color><color=white>" + restOfWord + "..." + "</color>";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }

            //Remove more from the back till it fits
            while (textWidth > bannerWidth)
            {
                //Remove a character from the back
                backNumToRemove++;
                shortCardName = shortCardName.Substring(0, shortCardName.Length - 1);

                //Recalculate size
                highlightedLetters = shortCardName.Substring(0, inputText.Length - frontNumToRemove);
                restOfWord = shortCardName.Substring(inputText.Length - frontNumToRemove);
                cardTextField.text = "<color=#3498DB>" + highlightedLetters + "</color><color=white>" + restOfWord + "..." + "</color>";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }

            //If there are no more to remove
            if (backNumToRemove == 0)
            {
                //Get rid of the ...
                highlightedLetters = shortCardName.Substring(0, inputText.Length - frontNumToRemove);
                restOfWord = shortCardName.Substring(inputText.Length - frontNumToRemove);
                cardTextField.text = "<color=#3498DB>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }
            */


        }
        else if (inputText.Length > 0)
        {
            //User has typed something but it doesn't match anything, we need to force a backspace
            inputText = inputText.Substring(0, inputText.Length - 1);
        }
        else
        {
            /*
            float textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
            float bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            while (textWidth > bannerWidth)
            {
                shortCardName = shortCardName.Substring(0, shortCardName.Length - 1);
                cardTextField.text = shortCardName + "...";
                textWidth = LayoutUtility.GetPreferredWidth(cardTextField.rectTransform);
                bannerWidth = ((cardTextField.transform.parent) as RectTransform).rect.width;
            }
            */
        }
        

    }
}
