using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    WaitForSeconds oneSec;

    public int maxTurns = 3;
    int currentTurn = 1;

    // Countdown
    public bool countdown;
    public int maxTimer = 60;
    int currentTimer;
    float internalTimer;

    // Start is called before the first frame update
    void Start()
    {

        // init for WaitForSeconds
        oneSec = new WaitForSeconds(1);

        // Start the "StartGame" Coroutine
        StartCoroutine("StartGame");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
