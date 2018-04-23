using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {


    public float tickInterval = 4f;

	void Start () {

        GlobalAttackCooldownTimer.instance.SetTickTime(tickInterval);
	}
	
    public void Win()
    {

        Debug.Log("You win the game");
        SceneTransitionManager.instance.PlayLevelTransitionSequence();
    }

    public void Draw()
    {
        Debug.Log("Ah, it was a draw");
        SceneTransitionManager.instance.PlayLevelTransitionSequence();
    }

    public void Lose()
    {
        
        SceneTransitionManager.instance.PlayDeathSequence();

    }

}
