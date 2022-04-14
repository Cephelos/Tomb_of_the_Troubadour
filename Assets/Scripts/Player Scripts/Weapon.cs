using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float dmg;
    private bool canDmg;
    public float attackDuriation;
    private float timeSinceAttack;

    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack > attackDuriation)
        {
            canDmg = false;
        }
    }

    public void Attack()
    {
        Debug.Log("attacked");
        canDmg = true;
        timeSinceAttack = 0;
    }

    public void StopAttack()
    {
        canDmg = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log("collided");
        if (canDmg && collision.CompareTag("Enemy"))
        {
            Debug.Log("damage");
            ///gameObject bg = other.gameObject;

            Destroy(collision.gameObject);
            canDmg = false;

        }

    }
}

