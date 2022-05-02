using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Weapon
{
    public float blastRadius = 10f;
    public SpriteRenderer explosionGraphic;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {

            Detonate();
        }
    }

    public override void Attack(PlayerMovement playerMovement)
    {
        // Don't play SFX!
        canDmg = true;
        timeSinceAttack = 0;
    }

    public void Detonate()
    {
        // Display graphic
        GameObject expGraphic = Instantiate(explosionGraphic, transform.position, Quaternion.identity).gameObject;
        expGraphic.transform.localScale = Vector3.one * blastRadius;
        // Destroy it after half a second (once animation is finished)
        Destroy(expGraphic, 0.4f);

        // Get colliders within blastRadius of the explosion center
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (Collider2D collider in collidersHit)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Hit enemy!");
                enemy.Destruct();
            }
        }

        // play SFX
        Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Explosion");
        // destroy projectile
        Destroy(gameObject);

    }
}