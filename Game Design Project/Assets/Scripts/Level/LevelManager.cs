using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using System;

public class LevelManager : MonoBehaviour
{

    WaitForSeconds oneSec;//we will be using this a lot so we don't want to create a new one everytime, saves a few bytes this
    public Vector3[] spawnPositions;// the positions characters will spawn on


    CameraManager camM;
    CharacterManager charM;
    LevelUI levelUI;//we store ui elements here for ease of access

    public int maxTurns = 2;
    int currentTurn = 1;//the current turn we are, start at 1

    //variables for the countdown
    public bool countdown;
    public int maxTurnTimer = 30;
    int currentTimer;
    float internalTimer;

    void Start()
    {
        //get the references from the singletons
        charM = CharacterManager.Instance;
        levelUI = LevelUI.Instance;
        camM = CameraManager.GetInstance();

        //init spawn positions
        spawnPositions = new Vector3[2];
        spawnPositions[0] = GameObject.FindGameObjectWithTag("Spawn1").transform.position;
        spawnPositions[1] = GameObject.FindGameObjectWithTag("Spawn2").transform.position;

        //init the WaitForSeconds
        oneSec = new WaitForSeconds(1);

        levelUI.AnnouncerTextLine.gameObject.SetActive(false);
        //levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        StartCoroutine("StartGame");

    }

    void FixedUpdate()
    {
        //A fast way to handle player orientation in the scene
        //just compare the x of the first player, if it's lower then the enemy is on the right

        if(charM != null && charM.plrs.Count == 2)
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

    void Update()
    {
        if (countdown)//if we enable countdown
        {
            HandleTurnTimer();//control the timer here
        }
    }

    void HandleTurnTimer()
    {
        levelUI.LevelTimer.text = currentTimer.ToString();

        internalTimer += Time.deltaTime; //every one second (frame dependant)

        if (internalTimer > 1)
        {
            currentTimer--; // substract from the current timer one
            internalTimer = 0;
        }

        if (currentTimer <= 0) //if the countdown is over
        {
            EndTurnFunction(true);//end the turn
            countdown = false;
        }
    }

    IEnumerator StartGame()
    {
        //when we first start the game

        //we need to create the plrs first
        yield return Createplrs();

        //then initialize the turn
        yield return InitTurn();
    }

    IEnumerator InitTurn()
    {
        //to init the turn

        //disable the announcer texts first
        levelUI.AnnouncerTextLine.gameObject.SetActive(false);
        //levelUI.AnnouncerTextLine2.gameObject.SetActive(false);

        //reset the timer
        currentTimer = maxTurnTimer;
        countdown = false;

        //start initiliazing the plrs
        yield return Initplrs();

        //and then start the coroutine to enable the controls of each player
        yield return EnableControl();

    }

    IEnumerator Createplrs()
    {
        //go to all the plrs we have in our list
        for (int i = 0; i < charM.plrs.Count; i++)
        {
            //and instantiate their prefabs
            GameObject go = Instantiate(charM.plrs[i].playerPrefab
            , spawnPositions[i], Quaternion.identity)
            as GameObject;

            //and assign the needed references
            charM.plrs[i].playerStates = go.GetComponent<StateManager>();

            charM.plrs[i].playerStates.healthSlider = levelUI.playerHealthbar[i];

            camM.players.Add(go.transform);
        }

        yield return null;
    }

    IEnumerator Initplrs()
    {
        //right now, the only thing we have to do is reset their health
        for (int i = 0; i < charM.plrs.Count; i++)
        {
            charM.plrs[i].playerStates.health = 100;
            charM.plrs[i].playerStates.handleAnim.anim.Play("Locomotion");
            //charM.plrs[i].playerStates.transform.GetComponent<Animator>().Play("Locomotion");
            charM.plrs[i].playerStates.transform.position = spawnPositions[i];
        }

        yield return null;
    }

    IEnumerator EnableControl()
    {
        //start with the announcer text

        levelUI.AnnouncerTextLine.gameObject.SetActive(true);
        levelUI.AnnouncerTextLine.text = "Turn " + currentTurn;
        levelUI.AnnouncerTextLine.color = Color.white;
        yield return oneSec;
        yield return oneSec;

        //change the UI text and color every second that passes
        levelUI.AnnouncerTextLine.text = "3";
        levelUI.AnnouncerTextLine.color = Color.green;
        yield return oneSec;
        levelUI.AnnouncerTextLine.text = "2";
        levelUI.AnnouncerTextLine.color = Color.yellow;
        yield return oneSec;
        levelUI.AnnouncerTextLine.text = "1";
        levelUI.AnnouncerTextLine.color = Color.red;
        yield return oneSec;
        levelUI.AnnouncerTextLine.color = Color.red;
        levelUI.AnnouncerTextLine.text = "FIGHT!";

        //and for every player enable what they need to have open to be controlled
        for (int i = 0; i < charM.plrs.Count; i++)
        {
            InputHandler ih = charM.plrs[i].playerStates.gameObject.GetComponent<InputHandler>();
            ih.playerInput = charM.plrs[i].inputId.ToString();
            ih.enabled = true;

            /*
            //for user plrs, enable the input handler for example
            if (charM.plrs[i].playerType == PlayerBase.PlayerType.user)
            {
                InputHandler ih = charM.plrs[i].playerStates.gameObject.GetComponent<InputHandler>();
                ih.playerInput = charM.plrs[i].inputId.ToString();
                ih.enabled = true;
            }

            //If it's an AI character
            
            if (charM.plrs[i].playerType == PlayerBase.PlayerType.ai)
            {
                AICharacter ai = charM.plrs[i].playerStates.gameObject.GetComponent<AICharacter>();
                ai.enabled = true;

                //assign the enemy states to be the one from the opposite player
                ai.enStates = charM.returnOppositePlater(charM.plrs[i]).playerStates;
            }
            */
        }

        //after a second, disable the announcer text
        yield return oneSec;
        levelUI.AnnouncerTextLine.gameObject.SetActive(false);
        countdown = true;
    }

    void DisableControl()
    {
        //to disable the controls, you need to disable the component that makes a character controllable
        for (int i = 0; i < charM.plrs.Count; i++)
        {
            //but first, reset the variables in their state manager 
            charM.plrs[i].playerStates.ResetStateInputs();

            charM.plrs[i].playerStates.GetComponent<InputHandler>().enabled = false;

            /*
            //for user plrs, that's the input handler
            if (charM.plrs[i].playerType == PlayerBase.PlayerType.user)
            {
                charM.plrs[i].playerStates.GetComponent<InputHandler>().enabled = false;
            }

            if (charM.plrs[i].playerType == PlayerBase.PlayerType.ai)
            {
                charM.plrs[i].playerStates.GetComponent<AICharacter>().enabled = false;
            }
            */
        }
    }

    public void EndTurnFunction(bool timeOut = false)
    {
        /* We call this function everytime we want to end the turn
         * but we need to know if we do so by a timeout or not
         */
        countdown = false;
        //reset the timer text
        levelUI.LevelTimer.text = maxTurnTimer.ToString();

        //if it's a timeout
        if (timeOut)
        {
            //add this text first
            levelUI.AnnouncerTextLine.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine.text = "Time Out!";
            levelUI.AnnouncerTextLine.color = Color.cyan;
        }
        else
        {
            levelUI.AnnouncerTextLine.gameObject.SetActive(true);
            levelUI.AnnouncerTextLine.text = "K.O.";
            levelUI.AnnouncerTextLine.color = Color.red;
        }

        //disable the controlls
        DisableControl();

        //and start the coroutine for end turn
        StartCoroutine("EndTurn");
    }

    IEnumerator EndTurn()
    {
        //wait 3 seconds for the previous text to clearly show
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        //find who was the player that won
        PlayerBase vPlayer = FindWinningPlayer();

        if (vPlayer == null) //if our function returned a null
        {
            //that means it's a draw
            levelUI.AnnouncerTextLine.text = "Draw";
            levelUI.AnnouncerTextLine.color = Color.blue;
        }
        else
        {
            //else that player is the winner
            levelUI.AnnouncerTextLine.text = vPlayer.playerId + " Wins!";
            levelUI.AnnouncerTextLine.color = Color.red;
        }

        //wait 3 more seconds
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        //check to see if the victorious player has taken any damage
        if (vPlayer != null)
        {
            //if not, then it's a flawless victory
            if (vPlayer.playerStates.health == 100)
            {
                levelUI.AnnouncerTextLine.gameObject.SetActive(true);
                levelUI.AnnouncerTextLine.text = "Flawless Victory!";
            }
        }

        //wait 3 seconds
        yield return oneSec;
        yield return oneSec;
        yield return oneSec;

        currentTurn++;//add to the turn counter

        bool matchOver = isMatchOver();

        if (!matchOver)
        {
            StartCoroutine("InitTurn"); // and start the loop for the next turn again
        }
        else
        {
            for (int i = 0; i < charM.plrs.Count; i++)
            {
                charM.plrs[i].score = 0;
                charM.plrs[i].hasCharacter = false;
            }

            if (charM.solo)
            {
                if (vPlayer == charM.plrs[0])
                    MySceneManager.GetInstance().LoadNextOnProgression();
                else
                    MySceneManager.GetInstance().RequestLevelLoad(SceneType.main, "game_over");
            }
            else
            {
                MySceneManager.GetInstance().RequestLevelLoad(SceneType.main, "select");
            }
        }
    }

    bool isMatchOver()
    {
        bool retVal = false;

        for (int i = 0; i < charM.plrs.Count; i++)
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
        //to find who won the turn
        PlayerBase retVal = null;

        StateManager targetPlayer = null;

        //check first to see if both plrs have equal health
        if (charM.plrs[0].playerStates.health != charM.plrs[1].playerStates.health)
        {
            //if not, then check who has the lower health, the other one is the winner
            if (charM.plrs[0].playerStates.health < charM.plrs[1].playerStates.health)
            {
                charM.plrs[1].score++;
                targetPlayer = charM.plrs[1].playerStates;
                levelUI.AddWinIndicator(1);
            }
            else
            {
                charM.plrs[0].score++;
                targetPlayer = charM.plrs[0].playerStates;
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
