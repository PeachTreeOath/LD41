using UnityEngine;
using UnityEditor;

public class CardPrototype : ScriptableObject {
    public string cardName;
    public int health;
    public int damage;

    //TODO add prefab for card image(s)
    public GameObject inLanePrefab;

    public Card Instantiate() {
        return new Card(this);
    }
}