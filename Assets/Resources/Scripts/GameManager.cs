using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

    private bool awaitingReset;
    public float tickInterval = 4f;

	void Start () {
        awaitingReset = false;
        GlobalAttackCooldownTimer.instance.SetTickTime(tickInterval);
	}
	
    public void Win()
    {
        if (awaitingReset) {
            Debug.LogError("Game is already transitioning, cannot win right now");
            return;
        }

        //TODO: Show an awesome animation or graphic because you won
        Debug.Log("You win the game");
        ResetGame();
    }

    public void Draw()
    {
        if(awaitingReset) {
            Debug.LogError("Game is already transitioning, cannot draw right now");
            return;
        }

        //TODO: Show a depressing lame graphic because draws are dumb
        Debug.Log("Ah, it was a draw");
        ResetGame();
    }

    public void Lose()
    {
        if(awaitingReset) {
            //Debug.LogError("Game is already transitioning, cannot lose right now");
            return;
        }

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
