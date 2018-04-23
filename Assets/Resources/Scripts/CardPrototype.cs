using UnityEngine;

public class CardPrototype : ScriptableObject {
    public string cardName;
    public int health;
    public int damage;
    public int timeToCast;
    public int cardsToDraw;
    public int casterHpToHeal;

    public Sprite sprite;
    public GameObject inLanePrefab;

    public CardModel Instantiate() {
        return new CardModel(this);
    }
}