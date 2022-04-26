using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    public GameObject sword;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("collided");
        Weapon weapon = sword.GetComponent<Weapon>();
        if (weapon.canDmg && collision.CompareTag("Enemy") && !collision.GetComponent<Enemy>().invincible)
        {
            Debug.Log("damage");
            ///gameObject bg = other.gameObject;

            collision.gameObject.GetComponent<Enemy>().Destruct();
            weapon.canDmg = false;

        }

    }
}
