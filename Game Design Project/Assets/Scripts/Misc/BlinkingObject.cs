using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingObject : MonoBehaviour
{
    private Renderer renderer;
    private Boolean blinking;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blinking)
        {
            Color oldCol = renderer.material.color;
            Color newCol = new Color(oldCol.r, oldCol.g, oldCol.b, oldCol.a - 0.01f);
            renderer.material.color = newCol;
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
