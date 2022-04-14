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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        if (canDmg)
        {
            Debug.Log("damage");
            ///gameObject bg = other.gameObject;
            if (other.gameObject.tag == "Enemy")
            {
                Destroy(other.gameObject);
                canDmg = false;
            }

        }

    }
}

