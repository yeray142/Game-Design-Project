using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Respawn2 : MonoBehaviour
{
    private GameObject playerObj = null;
    private float timeOnHold = 2;
    private bool respawned = false;
    private float initialGravity;
    private PlayerController moveScript;
    private CharacterController controller;
    private BlinkingObject blinkingObject;
    private GameObject spawn1;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj == null)
            playerObj = GameObject.FindGameObjectWithTag("Player2");

        spawn1 = GameObject.FindGameObjectWithTag("Spawn2");
        controller = playerObj.GetComponent<CharacterController>();
        moveScript = playerObj.GetComponent<PlayerController>();
        blinkingObject = playerObj.GetComponent<BlinkingObject>();
        initialGravity = moveScript.getGravity();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerObj != null)
        {

            if (respawned)
            {
                if (timeOnHold > 0)
                {
                    timeOnHold -= Time.deltaTime;
                }
                else
                {
                    blinkingObject.blinkOff();
                    controller.enabled = true;
                    moveScript.setGravity(initialGravity); // gravedad de vuelta
                    respawned = false;
                }
            }
            else if (playerObj.transform.position.y <= -7)
            {

                controller.enabled = false;
                Debug.Log("falling" + playerObj.transform.position.y);
                playerObj.transform.position = spawn1.transform.position;

                respawned = true;
                timeOnHold = 2;
                blinkingObject.blinkOn(); // Starts blinking
                moveScript.setGravity(0.0f);// gravedad a 0
            }
        }
    }
}