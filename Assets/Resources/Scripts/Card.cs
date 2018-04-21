public class Card {
    public CardPrototype prototype { get; private set; }
    public string name { get; private set; }
    public int health { get; private set; }
    public int damage { get; private set; }

    public Card(CardPrototype prototype) {
        this.prototype = prototype;
        this.name = prototype.cardName;
        this.health = prototype.health;
        this.damage = prototype.damage;
    }
}


