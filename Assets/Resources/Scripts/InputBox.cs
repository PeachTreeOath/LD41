using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputBox : MonoBehaviour {

    public InputField mainInputField;

    // Use this for initialization
    void Start()
    {
        GameObject canvasObj = GameObject.Find("Canvas");
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        mainInputField = canvas.GetComponentInChildren<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        //forces input to only the input box
        mainInputField.ActivateInputField();
    }
}
