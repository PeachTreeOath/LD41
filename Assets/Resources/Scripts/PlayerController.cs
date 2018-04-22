using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {

    public Health health;
    public TextMeshProUGUI healthText;
	
	// Update is called once per frame
	void Update () {
        healthText.text = health.current + "/" + health.max;
	}
}
