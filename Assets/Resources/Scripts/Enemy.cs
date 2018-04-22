using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Singleton<Enemy> {

    public GameObject discard;
    public GameObject playOrigin;
    public Card cardPrefab; //HACK this shouldn't just be sitting here...
}
