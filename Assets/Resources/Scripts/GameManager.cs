using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    public Health playerHealth;
    public Health enemyHealth;

    private bool awaitingReset;

	// Use this for initialization
	void Start () {
        awaitingReset = false;
        GlobalAttackCooldownTimer.instance.SetTickTime(2f);
	}
	
	// Update is called once per frame
	void Update () {
        if (!awaitingReset)
        {
            if (playerHealth.current <= 0 && enemyHealth.current <= 0)
            {
                Draw();
            }
            else if (playerHealth.current <= 0)
            {
                Lose();
            }
            else if (enemyHealth.current <= 0)
            {
                Win();
            }
        }
	}

    public void Win()
    {
        //TODO: Show an awesome animation or graphic because you won
        Debug.Log("You win the game");
        ResetGame();
    }

    public void Draw()
    {
        //TODO: Show a depressing lame graphic because draws are dumb
        Debug.Log("Ah, it was a draw");
        ResetGame();
    }

    public void Lose()
    {
        //TODO: Show the worst graphic ever cause you suck
        Debug.Log("Bummer, you lose");
        SceneTransitionManager.instance.PlayDeathSequence();
        ResetGame();
    }

    public void ResetGame()
    {
        //TODO: Do something here to reset the Scene?
        Debug.Log("The game needs to be reset");
        awaitingReset = true;
        //SceneManager.LoadScene("Game");
    }
}
