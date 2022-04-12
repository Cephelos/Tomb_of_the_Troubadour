using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer.Mechanics
{

public class EnemyCollision : MonoBehaviour
{
    private Rigidbody2D rBody;
    private Transform playerTransform;

    private PlayerMovement player;

    

    private Vector3 direction;


    [SerializeField] private float knockbackX = 5;
    [SerializeField] private float knockbackY = 5;
    [SerializeField] private int stunTime= 50;
    void Awake(){
        rBody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        player = GetComponent<PlayerMovement>();
        
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
            
        }

    }
}
}
