using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Pool : Singleton<Pool> {

	private List<Card> library = new List<Card>();
	private List<Card> pool = new List<Card>();
	private bool shouldShuffle = true;
	private bool isShuffled = false;

	//Shuffle the library
	public void Shuffle(){
		for (int i = 0; i < library.Count; i++) {
			int randomIndex = Random.Range(i, library.Count);

			Card temp = library[i];
			library[i] = library[randomIndex];
			library[randomIndex] = temp;
		}
	}

	//Add a card to library
	public void AddCard(Card newCard){
		library.Add(newCard);
	}

	//Fill an empty slot
	public void RefreshSlot(int slotNum) {
		//Shuffle when the first card is pulled
		if(shouldShuffle && !isShuffled) {
			Shuffle();
			isShuffled = true;
		}

		//Pop a card from the library
		Assert.IsTrue(library.Count > 0);
		pool[slotNum] = library[0];
		library.RemoveAt(0);
	}

	//Get value of card in slot
	public Card GetSlot(int slotNum) {
		return pool[slotNum];
	}

	public Card PopSlot(int slotNum) {
		Card retCard = pool[slotNum];
		pool[slotNum] = null;

		return retCard;
	}
}
