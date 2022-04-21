using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float dmg;
    private bool canDmg;
    public float attackDuriation;
    [SerializeField] private float timeSinceAttack;

    void Update()
    {       
        Renderer rend = gameObject.transform.GetComponent<Renderer>();
        Color matColor = rend.material.color;
        float a_value = rend.material.color.a;
        BoxCollider2D sword_Collider = gameObject.GetComponent<BoxCollider2D>();

        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack > attackDuriation)
        {
            canDmg = false;
            
        }
        if (!canDmg)
        {
            sword_Collider.size = new Vector2(0, 0);
            rend.material.color = new Color(matColor.r, matColor.g, matColor.b, 0);
        }
        else
        {
            sword_Collider.size = new Vector2(1, 1);
            rend.material.color = new Color(matColor.r, matColor.g, matColor.b, 0.7f);
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

    public bool CanDamage() // Is the weapon's attack active?
    {
        return canDmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("collided");
        if (canDmg && collision.CompareTag("Enemy"))
        {
            Debug.Log("damage");
            ///gameObject bg = other.gameObject;

            collision.gameObject.GetComponent<Enemy>().Destruct();
            canDmg = false;

        }

    }
}
