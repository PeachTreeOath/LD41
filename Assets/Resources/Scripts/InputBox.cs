using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputBox : MonoBehaviour {

    public InputField mainInputField;
    public Text testCardName;
    public string inputText;

    // Use this for initialization
    void Start()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        mainInputField = canvas.GetComponentInChildren<InputField>();
        testCardName = canvas.GetComponentInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        inputText = ((Text)mainInputField.GetComponentInChildren<Text>()).text;
    }
}
