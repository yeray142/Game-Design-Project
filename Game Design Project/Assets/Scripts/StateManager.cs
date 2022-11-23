using System.Collections;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public int health = 100;

    public float horizontal;
    public float vertical;
    public bool attack1;
    public bool attack2;
    public bool attack3;
    public bool crouch;

    public bool canAttack;
    public bool gettingHit;
    public bool currentlyAttacking;

    public bool dontMove;
    public bool onGround;
    public bool lookRight;

    public Slider healthSlider;
    SpriteRenderer sRenderer;

    /*
    [HideInInspector]
    public HandleDamageColliders handleDC;
    [HideInInspector]
    public HandleAnimations handleAnim;
    [HideInInspector]
    public HandleMovement handleMovement;
    */

    public GameObject[] movementColliders;

    // Start is called before the first frame update
    void Start()
    {
       // handleDC = GetComponent<HandleDamageColliders>();
       // handleAnim = GetComponent<HandleAnimations>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
