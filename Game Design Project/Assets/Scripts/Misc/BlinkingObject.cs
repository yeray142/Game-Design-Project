using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    private Renderer renderer;
    private Boolean blinking;
    private float blinkSpeed;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        blinkSpeed = 2.0f;
        originalColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (blinking)
        {
            
            Color oldCol = renderer.material.color;
            Color newCol = new Color(oldCol.r, oldCol.g, oldCol.b, oldCol.a);
            if (oldCol.a > 0)
            {
                newCol.a -= 0.01f;
            } else
            {
                newCol.a += 0.01f;
            }
            
            renderer.material.color = newCol;
        }
        else
        {
            renderer.material.color = originalColor;
        }
        
    }

    public void blinkOn()
    {
        blinking = true;
    }

    public void blinkOff()
    {
        blinking = false;
    }
}
