using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Pool : Singleton<Pool> {

	public int numInPool = 6;
	public List<CardPrototype> cardsToStart;

	private List<Card> library = new List<Card>();
	private GameObject[] gameObjects;
	private Card[] pool;
	private bool shouldShuffle = true;
	private bool isShuffled = false;

	void Start() {
		pool = new Card[numInPool];
		gameObjects = new GameObject[numInPool];

		for(int i = 0; i < gameObjects.Length; i++) {
			gameObjects[i] = Instantiate(Resources.Load("Prefabs/Card")) as GameObject;
			gameObjects[i].transform.parent = transform;
        }

        if (cardsToStart == null) {
            cardsToStart = new List<CardPrototype>();
        }

        for(int i = 0; i < numInPool; i++) {
            var card = cardsToStart[i];
            Card newCard = card.Instantiate();
            AddCard(newCard);
            gameObjects[i].GetComponent<CardView>().CreateCardImage(newCard);
        }

		//TODO: Make this somehow dynamic based on the number of cards
		gameObjects[0].transform.localPosition = new Vector3(0.5f, -1.0f, 0.0f);
		gameObjects[1].transform.localPosition = new Vector3(-0.5f, -1.0f, 0.0f);
		gameObjects[2].transform.localPosition = new Vector3(0.5f, 0.0f, 0.0f);
		gameObjects[3].transform.localPosition = new Vector3(-0.5f, 0.0f, 0.0f);
		gameObjects[4].transform.localPosition = new Vector3(0.5f, 1.0f, 0.0f);
		gameObjects[5].transform.localPosition = new Vector3(-0.5f, 1.0f, 0.0f);

		for(int i = 0; i < numInPool; i++) {
			RefreshSlot(i);
		}
	}

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
		pool[slotNum] = library[0];
		library.RemoveAt(0);

		gameObjects[slotNum].GetComponentInChildren<SpriteRenderer>().sprite = pool[slotNum].sprite;
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
