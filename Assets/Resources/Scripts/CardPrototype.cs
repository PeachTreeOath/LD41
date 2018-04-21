using UnityEngine;
using UnityEditor;

public class CardPrototype : ScriptableObject {
    public int health;
    public int damage;

    //TODO add prefab for card image(s)
    //TODO add prefab reference for lane

    public Card Instantiate() {
        return new Card(this);
    }
}