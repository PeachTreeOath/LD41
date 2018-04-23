using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour {

    public TextMeshProUGUI cardText;
    public Owner owner = Owner.None;
    public Vector2 playerPush = new Vector2(0, 1);
    public Vector2 enemyPush = new Vector2(1, 0);
    public float duration = 1f;

    private Rigidbody2D body;
    private float time = 0;

	void Start () {
        body = GetComponent<Rigidbody2D>(); //TODO: yeah I know this is lazy AF
        body.AddForce(owner == Owner.Enemy ? enemyPush : playerPush);
	}
	
	void Update () {
        time += Time.deltaTime;
        if(time >= duration) {
            Destroy(this.gameObject);
        } else { 
            cardText.alpha = 1f - time / duration;
        }

	}

    public void SetNumber(int amount) {
        cardText.text = "-" + amount;
    }
}
