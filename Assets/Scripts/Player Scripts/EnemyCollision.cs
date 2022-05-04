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
    private Health playerHealth;

    private PlayerMovement player;

    private Vector3 direction;

        private Weapon wep_script;

        private GameObject weapon;

    //public enum State { Attack, Move, Idle };

    [SerializeField] private float knockbackX = 5;
    [SerializeField] private float knockbackY = 5;
    [SerializeField] private float stunTime= 0.50f;
    [SerializeField] private float invincibleTime = 1f;
    void Awake(){
        rBody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        player = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<Health>();
        weapon = gameObject.transform.GetChild(2).gameObject;
        wep_script = weapon.GetComponent<Weapon>();

        }
    private void OnTriggerEnter2D(Collider2D collision)
    {
            
       /* if (collision.CompareTag("Trap") && !wep_script.canDmg && player.invincibleTimer == 0)
        {
            Debug.Log("tag" + collision.gameObject.tag);
            Vector3 enemyDir = (playerTransform.position - collision.gameObject.transform.position).normalized;
            if (enemyDir.x < 0)
            {
                knockbackX = -knockbackX;
            }

            rBody.velocity = Vector3.zero;
            rBody.AddForce(new Vector3(knockbackX, knockbackY, 0), ForceMode2D.Impulse);

            player.stunTimer = stunTime;
            player.invincibleTimer = invincibleTime;

            // Damage the player
            playerHealth.Decrement(collision.gameObject.GetComponent<Enemy>().contactDamage, transform.position);

            // Tell animator that the player has been hurt
            animator.SetBool("Hurt", true);

                // Play SFX
            GameController.Instance.audioController.PlaySFX("Player Hit");
            
        }*/
        if (collision.CompareTag("Lava")) {
            Debug.Log("Lava");
            Destroy (gameObject);

                // Play death SFX
            GameController.Instance.audioController.PlaySFX("Player Die");
        }

    }
}
}
