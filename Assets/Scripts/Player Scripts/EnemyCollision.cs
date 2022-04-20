using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Mechanics
{

public class EnemyCollision : MonoBehaviour
{
    private Rigidbody2D rBody;
    private Transform playerTransform;
    private Animator animator;
    private Health health;

    private PlayerMovement player;

    private Vector3 direction;


    [SerializeField] private float knockbackX = 5;
    [SerializeField] private float knockbackY = 5;
    [SerializeField] private float stunTime= 0.50f;
    void Awake(){
        rBody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        player = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            Vector3 enemyDir = (playerTransform.position - collision.gameObject.transform.position).normalized;
            if (enemyDir.x < 0)
            {
                knockbackX = -knockbackX;
            }

            rBody.velocity = Vector3.zero;
            rBody.AddForce(new Vector3(knockbackX, knockbackY, 0), ForceMode2D.Impulse);

            player.stunTimer = stunTime;

            // Damage the player
            health.Decrement();

            // Tell animator that the player has been hurt
            animator.SetBool("Hurt", true);
            
        }
        if (collision.CompareTag("Lava")) {
            Destroy (gameObject);
        }

    }
}
}
