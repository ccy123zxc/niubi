using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDead : MonoBehaviour
{
    public bool isDead = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            isDead = true;
        }
    }
}
