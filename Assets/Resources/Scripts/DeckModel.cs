using System.Collections.Generic;
using UnityEngine;

public class DeckModel : MonoBehaviour {

    public List<CardModel> library = new List<CardModel>();
    public List<CardModel> discard = new List<CardModel>();

    public void PutOnTop(CardModel card) {
        library.Add(card);
    }

    public void Shuffle(bool addDiscard=true) {
        if(addDiscard) {
            library.AddRange(discard);
            if(discard.Count > 0)
            {
                //This will technically make the cards move to the deck at the same time a
                //Card comes into the hand when the deck is empty, but it moves so fast it looks fine
                Card card = transform.Find("Slot").GetComponent<ObjectSlot>().objectInSlot.GetComponent<Card>();
                transform.Find("Slot").GetComponent<ObjectSlot>().Release();
                card.MoveToDeck();
                transform.GetComponent<Deck>().Clear();
            }
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

    public CardModel Draw() {
        CardModel card = null;

        if(library.Count == 0) {
            Shuffle();
            //TODO notify the system there's been a shuffle!
        }

        if(library.Count > 0) {
            var index = library.Count - 1;
            card = library[index];
            library.RemoveAt(index);
        }

        return card;
    }

    public int DrawMultiple(int count, List<CardModel> dest) {
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

    public void Discard(CardModel card) {
        discard.Add(card);
    }
}
