using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    WaitForSeconds oneSec;
    private Vector3[] spawns; // Spawn positions

    CharacterManager charM;
    LevelUI levelUI;

    public int maxTurns = 3;
    int currentTurn = 1;

    // Variables for countdown
    public bool countdown;
    public int maxTurnTimer = 60;
    int currentTimer;
    float internalTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Get the references from the Singletons
        charM = CharacterManager.Instance;
        levelUI = LevelUI.Instance;

        // init for WaitForSeconds
        oneSec = new WaitForSeconds(1);

        // Get the Spawn positions
        spawns = new Vector3[2];
        spawns[0] = GameObject.FindGameObjectWithTag("Spawn1").transform.position;
        spawns[1] = GameObject.FindGameObjectWithTag("Spawn2").transform.position;

        // Disable the announcer text
        levelUI.AnnouncerTextLine.gameObject.SetActive(false);

        // Start the "StartGame" Coroutine
        StartCoroutine("StartGame");
    }

    void FixedUpdate()
    {
        // A fast way to handle player orientation in the scene
        // Just compare the x of the first player, if it's lower than the enemy is on the right
        if(charM.plrs.Count > 0)
        {
            if (charM.plrs[0].playerStates.transform.position.x <
            charM.plrs[1].playerStates.transform.position.x)
            {
                charM.plrs[0].playerStates.lookRight = true;
                charM.plrs[1].playerStates.lookRight = false;
            }
            else
            {
                charM.plrs[0].playerStates.lookRight = false;
                charM.plrs[1].playerStates.lookRight = true;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown) // If it's on countdown
        {
            HandleTurnTimer(); // Control the timer
        }
        
    }

    void HandleTurnTimer()
    {
        levelUI.LevelTimer.text = currentTimer.ToString();

        internalTimer += Time.deltaTime; // Every one second (frame dependant)

        if(internalTimer > 1)
        {
            currentTimer--;
            internalTimer = 0;
        }

        if(currentTimer <= 0)
        {
            EndTurnFunction(true); // End the turn
            countdown = false;
        }
    }

    IEnumerator StartGame()
    {
        // When we first start the game

        // We need to create the players first
        yield return CreatePlayers();

        // Initialize the turn
        yield return InitTurn();
    }

    IEnumerator InitTurn()
    {
        // To init the turn

        // Disable the announcer texts first
        levelUI.AnnouncerTextLine.gameObject.SetActive(false);

        // Reset the timer
        currentTimer = maxTurnTimer;
        countdown = false;

        // Start initializing the players
        yield return InitPlayers();

        // And then start the coroutine to enable the controls of each player
        yield return EnableControl();

    }

    IEnumerator InitPlayers()
    {
        for(int i = 0; i < charM.plrs.Count; i++)
        {
            
            charM.plrs[i].playerStates.health = 100;
            //charM.plrs[i].playerStates.handleAnim.anim.Play("Locomotion");
            charM.plrs[i].playerStates.transform.position = spawns[i];
            
        }

        yield return new WaitForEndOfFrame();
    }

    IEnumerator CreatePlayers()
    {
        for (int i = 0; i < charM.plrs.Count; i++)
        {
            GameObject go = Instantiate(
                charM.plrs[i].playerPrefab,
                spawns[i],
                Quaternion.identity
                ) as GameObject;

            charM.plrs[i].playerStates = go.GetComponent <StateManager>();
            charM.plrs[i].playerStates.healthSlider = levelUI.playerHealthbar[i];
        }

        yield return null;

    }

    IEnumerator EnableControl()
    {

        // Start with the announcer text
        levelUI.AnnouncerTextLine.gameObject.SetActive(true);
        levelUI.AnnouncerTextLine.text = "Turn " + currentTurn;
        levelUI.AnnouncerTextLine.color = Color.white;
        yield return oneSec;
        yield return oneSec;

        // Change the UI Text and Color every second that passes
        levelUI.AnnouncerTextLine.text = "3";
        levelUI.AnnouncerTextLine.color = Color.green;
        yield return oneSec;
        levelUI.AnnouncerTextLine.text = "2";
        levelUI.AnnouncerTextLine.color = Color.yellow;
        yield return oneSec;
        levelUI.AnnouncerTextLine.text = "1";
        levelUI.AnnouncerTextLine.color = Color.blue;
        yield return oneSec;
        levelUI.AnnouncerTextLine.text = "FIGHT!";
        levelUI.AnnouncerTextLine.color = Color.red;

        // And for every player enable what they need to have open to be controlled
        for(int i = 0; i < charM.plrs.Count; i++)
        {
            // For user players, enable the input handler for example
            PlayerController controller = charM.plrs[i].playerStates.gameObject.GetComponent<PlayerController>();
            controller.enabled = true;
        }

        // After a second, disable the announcer text
        yield return oneSec;
        levelUI.AnnouncerTextLine.gameObject.SetActive(false);
        countdown = true;

    }

    void DisableControl()
    {
        for(int i = 0; i < charM.plrs.Count; i++)
        {
            // First, reset the state manager variables
            charM.plrs[i].playerStates.ResetStateInputs();
            charM.plrs[i].playerStates.GetComponent<PlayerController>().enabled = false;
        }
    }

    public void EndTurnFunction(bool timeOut = false)
    {
        levelUI.LevelTimer.text = maxTurnTimer.ToString();

        if (timeOut)
        {
            levelUI.AnnouncerTextLine.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine.text = "Time Out!";
            levelUI.AnnouncerTextLine.color = Color.red;
        }
        else
        {
            levelUI.AnnouncerTextLine.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine.text = "K.O.";
            levelUI.AnnouncerTextLine.color = Color.red;
        }

        DisableControl();
    }

    IEnumerator EndTurn()
    {
        // Wait 3 seconds for the previous text to clearly show
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        // Find who's the player that won
        PlayerBase vPlayer = FindWinningPlayer();
        
        if(vPlayer == null)
        {
            levelUI.AnnouncerTextLine.text = "Draw";
            levelUI.AnnouncerTextLine.color = Color.cyan;
        }
        else
        {
            levelUI.AnnouncerTextLine.text = vPlayer.playerId + " Wins!";
            levelUI.AnnouncerTextLine.color = Color.red;

            // Wait 3 more seconds
            yield return oneSec;
            yield return oneSec;
            yield return oneSec;

            if(vPlayer.playerStates.health == 100)
            {
                levelUI.AnnouncerTextLine.text = "Flawless Victory!";
                levelUI.AnnouncerTextLine.color = Color.yellow;
            }
        }

        // Wait 3 more seconds
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        currentTurn++;

        bool matchOver = isMatchOver();

        if (!matchOver)
        {
            StartCoroutine("InitTurn");
        }
        else
        {
            for(int i = 0; i < charM.plrs.Count; i++)
            {
                charM.plrs[i].score = 0;
                charM.plrs[i].hasCharacter = false;
            }

            SceneManager.LoadSceneAsync("MainMenu");
        }


    }

    bool isMatchOver()
    {
        bool retVal = false;

        for(int i = 0; i < charM.plrs.Count; i++)
        {
            if (charM.plrs[i].score >= maxTurns)
            {
                retVal = true;
                break;
            }
        }

        return retVal;
    }

    PlayerBase FindWinningPlayer()
    {
        PlayerBase retVal = null;

        
        PlayerBase player1 = charM.plrs[0];
        PlayerBase player2 = charM.plrs[1];

        if (player1.playerStates.health != player2.playerStates.health)
        {
            StateManager targetPlayer = null;

            if (player1.playerStates.health < player2.playerStates.health)
            {
                player2.score++;
                targetPlayer = player2.playerStates;
                levelUI.AddWinIndicator(1);
            }
            else
            {
                player1.score++;
                targetPlayer = player1.playerStates;
                levelUI.AddWinIndicator(0);
            }

            retVal = charM.returnPlayerFromStates(targetPlayer);
        }

        return retVal;
    }

    public static LevelManager instance;
    public static LevelManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
}
