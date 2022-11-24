using System.Collections;
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

    public PlayerController handleMovement;

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
       // handleMovement = GetComponent<HandleMovement>();

        sRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        sRenderer.flipX = lookRight;

        onGround = isOnGround();

        if (healthSlider != null)
            healthSlider.value = health * 0.01f;

        if (health <= 0)
        {
            
            if (LevelManager.GetInstance().countdown)
            {
                LevelManager.GetInstance().EndTurnFunction();
                //handleAnim.anim.Play("Dead");
            }
            
        }
    }

    bool isOnGround()
    {
        bool ret = false;

        LayerMask layer = ~(1 << gameObject.layer | 1 << 3);
        ret = Physics2D.Raycast(transform.position, -Vector2.up, 0.1f, layer);

        return ret;
    }

    public void ResetStateInputs()
    {
        horizontal = 0;
        vertical = 0;
        attack1 = false;
        attack2 = false;
        attack3 = false;
        crouch = false;
        gettingHit = false;
        currentlyAttacking = false;
        dontMove = false;
    }

    public void CloseMovementCollider(int i)
    {
        movementColliders[i].SetActive(false);
    }

    public void OpenMovementCollider(int i)
    {
        movementColliders[i].SetActive(true);
    }

    /*
    public void TakeDamage(int damage, HandleDamageColliders.DamageType damageType)
    {
        if (!gettingHit)
        {
            switch (damageType)
            {
                case HandleDamageColliders.DamageType.ligth:
                    StartCoroutine(CloseInmortality(0.3f));
                    break;
                case HandleDamageColliders.DamageType.heavy:
                    handleMovement.AddVelocityOnCharacter(
                        ((!lookRight) ? Vector3.right * 1 : Vector3.right * -1) + Vector3.up,
                        0.5f
                        );
                    StartCoroutine(CloseInmortality(1));
                    break;
            }

            health -= damage;
            gettingHit = true;
        }
    }
    */

    IEnumerator CloseInmortality(float timer)
    {
        yield return new WaitForSeconds(timer);
        gettingHit = false;
    }
}
