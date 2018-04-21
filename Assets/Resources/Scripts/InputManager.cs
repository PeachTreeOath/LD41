using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : Singleton<InputManager>
{

    //This is a test case
    public string cardText = "New Text";

    private List<IInputListener> listeners = new List<IInputListener>();
    private string inputText = "";
    private TextMeshProUGUI textField;
    /*
	// Use this for initialization
	void Start () {
        GameObject canvasObj = GameObject.Find("Canvas");
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        textField = canvas.GetComponentInChildren<TextMeshProUGUI>();
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
        if (inputText.Length > 0 && cardText.StartsWith(inputText))
        {
            string highlightedLetters = inputText;
            string restOfWord = cardText.Substring(inputText.Length);

            textField.text = "<color=red>" + highlightedLetters + "</color><color=white>" + restOfWord + "</color>";
            print("match!");
        }
    }

    public void RegisterListener(IInputListener listener)
    {
        listeners.Add(listener);
    }
    */
}
