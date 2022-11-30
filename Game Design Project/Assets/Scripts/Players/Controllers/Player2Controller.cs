using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed; // 10
    [SerializeField]
    private float _gravity; // 0.11
    [SerializeField]
    private float _jumpHeight; // 20

    private float _yVelocity;
    private bool _canDoubleJump;

    private KeyCode upKey = KeyCode.UpArrow;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_controller != null)
        {
            float hInput = Input.GetAxis("Horizontal2");
            Vector3 direction = new Vector3(hInput, 0, 0);
            Vector3 velocity = direction * _speed;


            if (_controller.isGrounded)
            {
                if (Input.GetKeyDown(upKey))
                {
                    _yVelocity = _jumpHeight;
                    _canDoubleJump = true;
                }
            }
            else
            {
                if (_canDoubleJump && Input.GetKeyDown(upKey))
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