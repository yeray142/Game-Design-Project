using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public CharacterController controller;
    private Vector3 direction;
    public float speed = 8;
    public float gravity = 9.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        direction.x = hInput * speed;

        float vInput = Input.GetAxis("Vertical");
        
        //direction.y = hInput * speed;

        controller.Move(direction * Time.deltaTime);
    }
}
