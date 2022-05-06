using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int dmg;
    public bool canDmg;
    public float attackDuriation;
    public AudioClip attackSound;
    [SerializeField] public float timeSinceAttack;

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

    public virtual void Attack(Platformer.Mechanics.PlayerMovement playerMovement)
    {
        canDmg = true;
        timeSinceAttack = 0;
        Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Slash");
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
        if (canDmg && collision.CompareTag("Enemy") && collision.GetComponent<EnemyHealth>().enemyCanDie)
        {
            Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Hit");
            collision.gameObject.GetComponent<EnemyHealth>().Decrement(dmg, transform.position);
            canDmg = false;
        }
    }
}