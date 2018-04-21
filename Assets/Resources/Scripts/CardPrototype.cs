using UnityEngine;
using UnityEditor;

public class CardPrototype : ScriptableObject {
    public string cardName;
    public int health;
    public int damage;
    public Sprite sprite;

    //TODO add prefab for card image(s)
    public GameObject inLanePrefab;

    public CardModel Instantiate() {
        return new CardModel(this);
    }
}