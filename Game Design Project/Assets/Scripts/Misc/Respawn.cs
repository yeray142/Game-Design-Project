using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

public class Respawn : MonoBehaviour
{
    private GameObject playerObj = null;
    private float timeOnHold = 2;
    private bool respawned = false;
    private InputHandler ih;
    private Rigidbody2D rb;
    private BlinkingObject blinkingObject;
    private GameObject spawn;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj == null)
            playerObj = this.gameObject;

        rb = GetComponent<Rigidbody2D>();
        spawn = GameObject.FindGameObjectWithTag("Spawn1");
        ih = playerObj.GetComponent<InputHandler>();
        blinkingObject = playerObj.GetComponent<BlinkingObject>();

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
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    ih.enabled = true;
                    respawned = false;
                }
            }
            else if (playerObj.transform.position.y <= -7)
            {

                ih.enabled = false;
                playerObj.transform.position = spawn.transform.position;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                respawned = true;
                timeOnHold = 2;
                blinkingObject.blinkOn(); // Starts blinking
            }
        }
    }
}