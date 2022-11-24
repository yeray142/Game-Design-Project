using System.Collections;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    StateManager states;

    public HandleDamageCollider.DamageType damageType;

    private void Start()
    {
        states = GetComponentInParent<StateManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<StateManager>())
        {
            StateManager oState = collision.GetComponentInParent<StateManager>();

            if (oState != states)
            {
                // oState.TakeDamage(30, damageType);
            }
        }
    }
}
