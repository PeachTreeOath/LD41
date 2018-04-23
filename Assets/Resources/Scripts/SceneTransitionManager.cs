using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {

    public float secondsToFade;
    public float secondsToRevealHorizontalLine;
    public float secondsToDropCutsceneCardIn;
    public float secondsToPauseBeforeSceneTransition;
    public float secondsToPauseOnDeath;

    private bool endingStarted = false;
    private bool deathStarted = false;
    private float endingTime;
    private bool horizontalWipeDone = false;
    private bool fadeoutDone = false;
    private bool dropCutsceneCardDone = false;
    private bool finalPauseDone = false;
    private bool coroutineInProgress = false;


    CutsceneCard gameOverCard;
    CutsceneCard levelCompleteCard;
    CutsceneCard titleScreenCard;
    SpriteRenderer whiteMask;


    public void PlayLevelTransitionSequence()
    {
        endingTime = 0f;
        endingStarted = true;
        //gameOverText.enabled = true;

        //kicks off these coroutines
        Color color = Color.black;
        color.a = 0;
        whiteMask.color = color;

        //FadeUI.instance.FadeMe();
        FadeSprite.instance.FadeMe();
        GlobalTimer.instance.PauseTimer(true);

    }



  
    public void PlayDeathSequence()
    {
        //print("in PlayDeathSequence");
        //AudioManager.instance.PlayMusicOnce("LoseTheme", AudioManager.instance.GetMusicVolume());
        endingTime = 0f;
        deathStarted = true;
        //gameOverText.enabled = true;

        //kicks off these coroutines
        Color color = Color.black;
        color.a = 0;
        whiteMask.color = color;

        //FadeUI.instance.FadeMe();
        FadeSprite.instance.FadeMe();
        GlobalTimer.instance.PauseTimer(true);

    }

    IEnumerator DoDropCutsceneCard(CutsceneCard cutsceneCard)
    {
        //print("In DoDropCutsceneCard");
        float dropCutsceneCardTime = 0f;
        float dropCutsceneCardDistance = 10f; //needs to be adjusted to length of the line
        
        Vector3 startingPos = cutsceneCard.transform.localPosition;
        float currentPosY = startingPos.y;


        while (Mathf.Abs(startingPos.y - currentPosY) < dropCutsceneCardDistance)
        {
            dropCutsceneCardTime += Time.deltaTime;
            Debug.Log("startingPos.y = " + startingPos.y);
            currentPosY = startingPos.y - (dropCutsceneCardDistance * dropCutsceneCardTime) / secondsToDropCutsceneCardIn;

            Vector3 newPos = cutsceneCard.transform.localPosition;
            //Vector3 newCamPos = Camera.main.transform.localPosition;

            newPos.y = currentPosY;
            //newCamPos.y = cameraCurrentPosY;

            cutsceneCard.transform.localPosition = newPos;
            //Camera.main.transform.localPosition = newCamPos;
            yield return null;
        }
        dropCutsceneCardDone = true;
        coroutineInProgress = false;
        yield return null;
    }
    
    void Start()
    {

    }

    protected override void Awake()
    {
        base.Awake();
        Scene scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;

        if (scene.name.Equals("TitleScreen"))
        {
            titleScreenCard = GameObject.Find("TitleScreenCard").GetComponent<CutsceneCard>();
        }
        else
        {
            gameOverCard = GameObject.Find("GameOverCard").GetComponent<CutsceneCard>();
            levelCompleteCard = GameObject.Find("LevelCompleteCard").GetComponent<CutsceneCard>();
            whiteMask = GameObject.Find("White Mask").GetComponent<SpriteRenderer>();

            switch (sceneName)
            {
                case "Game":
                    levelCompleteCard.nextScene = "Level 2";
                    gameOverCard.nextScene = "Game";
                    break;
                case "Level 2":
                    levelCompleteCard.nextScene = "Level 3";
                    gameOverCard.nextScene = "Level 2";
                    break;
                case "Level 3":
                    levelCompleteCard.nextScene = "TitleScreen";
                    gameOverCard.nextScene = "Level 3";
                    break;
                default:
                    levelCompleteCard.nextScene = "Game";
                    gameOverCard.nextScene = "Game";
                    break;
            }
        }
            
    }


    void Update()
    {

        if (endingStarted)
        {
            endingTime += Time.deltaTime;

            if (endingTime >= secondsToFade && !dropCutsceneCardDone && !coroutineInProgress)
            {
                endingTime = 0f;
                coroutineInProgress = true;
                //kickoff horizontalWipe
                StartCoroutine(DoDropCutsceneCard(levelCompleteCard));
                InputManager.instance.inVictorySequence = true;
            }
            else if (endingTime >= secondsToDropCutsceneCardIn && !finalPauseDone && !coroutineInProgress)
            {
                //kickoff finalPause
                endingTime = 0f;
                finalPauseDone = true;
            }
            else if (endingTime >= secondsToPauseOnDeath && !coroutineInProgress)
            {
                //SceneManager.LoadScene("Game");
            }
        }
        else if (deathStarted)
        {
            //print("death sequence started");
            endingTime += Time.deltaTime;

            if (endingTime >= secondsToFade && !dropCutsceneCardDone && !coroutineInProgress)
            {
                endingTime = 0f;
                coroutineInProgress = true;
                //kickoff horizontalWipe
                StartCoroutine(DoDropCutsceneCard(gameOverCard));
                InputManager.instance.inGameOverSequence = true;
            }
            else if (endingTime >= secondsToDropCutsceneCardIn && !finalPauseDone && !coroutineInProgress)
            {
                //kickoff finalPause
                endingTime = 0f;
                finalPauseDone = true;
            }
        }
        
    }

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
