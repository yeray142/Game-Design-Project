using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Boolean blinking;
    private float blinkSpeed;
    private Color originalColor;

    public float spriteBlinkingTimer = 0.0f;
    public float spriteBlinkingMiniDuration = 0.1f;
    public float spriteBlinkingTotalTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponentInChildren(typeof(SpriteRenderer)) as SpriteRenderer;
        blinkSpeed = 0.5f;
        originalColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (blinking)
        {

            /*Color oldCol = renderer.material.color;
            Color newCol = oldCol;
            if (oldCol.a > 0)
            {
                newCol.a -= blinkSpeed * 0.01f / 2;
            } else
            {
                newCol.a += blinkSpeed * 0.01f / 2;
            }
            
            renderer.color = newCol;*/
           
            spriteBlinkingTimer += Time.deltaTime;

            if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
            {
                spriteBlinkingTimer = 0.0f;
                if (renderer.enabled == true)
                {
                    renderer.enabled = false;  //make changes
                }
                else
                {
                    renderer.enabled = true;   //make changes
                }
            }
        }
        else
        {
            renderer.enabled = true;
        }
        
    }

    public void blinkOn()
    {
        spriteBlinkingTotalTimer += Time.deltaTime;
        blinking = true;
    }

    public void blinkOff()
    {
        blinking = false;
        spriteBlinkingTotalTimer = 0.0f;
        renderer.enabled = true;   // according to 
    }
}
