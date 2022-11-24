using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed; // 10
    [SerializeField]
    private float _gravity; // 0.11
    [SerializeField]
    private float _jumpHeight; // 18

    private float _yVelocity;
    private bool _canDoubleJump;

    public int playerInput;
    private KeyCode[] upKeys = { KeyCode.W, KeyCode.UpArrow };

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInput != null && (playerInput == 0 || playerInput == 1))
        {
            float hInput = 0;
            if (playerInput == 0)
            {
                hInput = Input.GetAxis("Horizontal");
            } else if(playerInput == 1)
            {
                hInput = Input.GetAxis("Horizontal2");
            }
            Vector3 direction = new Vector3(hInput, 0, 0);
            Vector3 velocity = direction * _speed;


            if (_controller.isGrounded)
            {
                if (Input.GetKeyDown(upKeys[playerInput]))
                {
                    _yVelocity = _jumpHeight;
                    _canDoubleJump = true;
                }
            }
            else
            {
                if (_canDoubleJump && Input.GetKeyDown(upKeys[playerInput]))
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
                _yVelocity -= _gravity;
            }

            velocity.y = _yVelocity;

            _controller.Move(velocity * Time.deltaTime);
        }
        
    }

   public void setGravity(float gravity)
    {
        _gravity = gravity;
    }

    public float getGravity()
    {
        return _gravity;
    }
}
