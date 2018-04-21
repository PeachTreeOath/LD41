using UnityEngine;
using System.Collections.Generic;

public class Deck : Singleton<Deck> {

    public List<Card> library = new List<Card>();
    public List<Card> discard = new List<Card>();

    public void PutOnTop(Card card) {
        library.Add(card);
    }

    public void Shuffle(bool addDiscard=true) {
        if(addDiscard) {
            library.AddRange(discard);
            discard.Clear();
        }

        //TODO turn this into a static extension?
        var count = library.Count;
        var last = count - 1;
        for(var i = 0; i < last; i++) {
            var randomIndex = Random.Range(i, count);
            var temp = library[i];
            library[i] = library[randomIndex];
            library[randomIndex] = temp;
        }
    }

    public Card Draw() {
        Card card = null;

        if(library.Count > 0) {
            var index = library.Count - 1;
            card = library[index];
            library.RemoveAt(index);
        }

        return card;
    }

    public int DrawMultiple(int count, List<Card> dest) {
        int drawn = 0;
        for(var i = 0; i < count; i++) {
            var card = Draw();
            if(card != null) {
                dest.Add(card);
                drawn++;
            }
        }

        return drawn;
    }

    public void Discard(Card card) {
        discard.Add(card);        
    }
}
