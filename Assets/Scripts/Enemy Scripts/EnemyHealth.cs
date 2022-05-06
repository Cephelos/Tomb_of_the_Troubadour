using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public bool enemyCanDie = false;
    public bool IsAlive => currentHP > 0;

    public int currentHP;
    [SerializeField] private int maxHP;
    [SerializeField] private float knockbackX = 5;
    [SerializeField] private float knockbackY = 5;

    private Animator animator;
    private HealthBar healthBar;
    private bool died;

    private Rigidbody2D rBody;
    private Transform enemyTransform;
    private Vector3 direction;

    void Awake()
    {   
        animator = gameObject.GetComponent<Animator>();
        healthBar = gameObject.GetComponentsInChildren<HealthBar>()[0];
        enemyTransform = GetComponent<Transform>();
        rBody = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    public void Increment(int incrementValue)
    {
        currentHP = Mathf.Clamp(currentHP + incrementValue, 0, maxHP);
    }

    public void Decrement(int decrementValue, Vector3 player_pos, float knockbackX = 5, float knockbackY = 5)
    {
        Debug.Log("hit!");
        currentHP = Mathf.Clamp(currentHP - decrementValue, 0, maxHP);
        if (gameObject.tag == "Enemy")
        {
            healthBar.DecreaseHealthBar(currentHP, maxHP);
        }
        if (currentHP == 0 && enemyCanDie)
        {
            if (!died)
            {
                animator.SetTrigger("Death");
                died = true;
                Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Skeleton Die");
            }
        }
        else
        {
            Debug.Log("stunning");
            animator.SetBool("Stun", true);
            Vector3 enemyDir = (enemyTransform.position - player_pos).normalized;
            if (enemyDir.x < 0)
            {
                knockbackX = -knockbackX;
            }
            rBody.velocity = Vector3.zero;
            rBody.AddForce(new Vector3(knockbackX, knockbackY, 0), ForceMode2D.Impulse);
        }
    }

    public void DeathAnimationEnded()
    {
        Platformer.Mechanics.GameController.Instance.currentRoom.spawnEnemies.activeEnemies.Remove(gameObject.GetComponent<Enemy>());
        Destroy(gameObject);
    }

    public void StunAnimationEnded()
    {
        Debug.Log("stun animation ended");
        animator.SetBool("Stun", false);
    }
}