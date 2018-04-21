using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Pool : Singleton<Pool> {

	public List<CardPrototype> cardsInLibrary = new List<CardPrototype>();

	private List<CardModel> library = new List<CardModel>();
	private List<GameObject> gameObjects = new List<GameObject>();
	private List<CardModel> pool = new List<CardModel>();
	private bool shouldShuffle = true;
	private bool isShuffled = false;

	void Start() {
		//Get instances of the game objects from the editor
		foreach (Transform child in transform)
		{
			gameObjects.Add(child.gameObject);
		}

		//Load all the cards into the library
        for(int i = 0; i < numInPool; i++) {
            CardPrototype cardProt = cardsInLibrary[i];
            CardModel card = cardProt.Instantiate();
            AddCard(card);
            gameObjects[i].GetComponent<CardView>().CreateCardImage(card);
        }

        //Pull cards and place them into the pool
		for(int i = 0; i < numInPool; i++) {
			RefreshSlot(i);
		}
	}

	//Shuffle the library
	public void Shuffle(){
		for (int i = 0; i < library.Count; i++) {
			int randomIndex = Random.Range(i, library.Count);

			CardModel temp = library[i];
			library[i] = library[randomIndex];
			library[randomIndex] = temp;
		}
	}

	//Add a card to library
	public void AddCard(CardModel newCard){
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
		pool[slotNum] = library[0];
		library.RemoveAt(0);

		gameObjects[slotNum].GetComponentInChildren<SpriteRenderer>().sprite = pool[slotNum].sprite;
	}

	//Get value of card in slot
	public CardModel GetSlot(int slotNum) {
		return pool[slotNum];
	}

	public CardModel PopSlot(int slotNum) {
		CardModel retCard = pool[slotNum];
		pool[slotNum] = null;

		return retCard;
	}
}
