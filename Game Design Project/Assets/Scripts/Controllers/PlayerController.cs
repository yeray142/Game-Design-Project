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

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(hInput, 0, 0);
        Vector3 velocity = direction * _speed;

        
        if (_controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        } else
        {
            if(_canDoubleJump && Input.GetKeyDown(KeyCode.UpArrow))
            {
                _yVelocity += _jumpHeight;
                _canDoubleJump = false;
            }
            _yVelocity -= _gravity;
        }

        velocity.y = _yVelocity;

        _controller.Move(velocity * Time.deltaTime);
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
