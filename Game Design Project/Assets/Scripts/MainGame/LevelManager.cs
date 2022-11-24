using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    WaitForSeconds oneSec;
    private Vector3[] spawns;// Spawn positions

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
        charM = CharacterManager.GetInstance();
        levelUI = LevelUI.Instance;

        // init for WaitForSeconds
        oneSec = new WaitForSeconds(1);

        // Get the Spawn positions
        spawns[0] = GameObject.FindGameObjectWithTag("Spawn1").transform.position;
        spawns[1] = GameObject.FindGameObjectWithTag("Spawn2").transform.position;

        // Disable the announcer text
        levelUI.AnnouncerTextLine.gameObject.SetActive(false);

        // Start the "StartGame" Coroutine
        StartCoroutine("StartGame");
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleTurnTimer()
    {

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
            /*
            charM.plrs[i].playerStates.health = 100;
            charM.plrs[i].playerStates.handleAnim.anim.Play("Locomotion");
            charM.plrs[i].playerStates.transform.position = spawnPositions[i].position;
            */
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

            // charM.plrs[i].playerStates = go.GetComponent < StateManager() >;
            // charM.plrs[i].playerStates.healthSlider = levelUI.getPlayerTexts()[i];
        }

        yield return new WaitForEndOfFrame();

    }

    IEnumerator EnableControl()
    {

    }
}
