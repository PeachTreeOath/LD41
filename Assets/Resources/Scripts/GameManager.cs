﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	// Use this for initialization
	void Start () {
        GlobalAttackCooldownTimer.instance.SetTickTime(2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
