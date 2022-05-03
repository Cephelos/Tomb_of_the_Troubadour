using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP;
    public bool playerCanDie = false;
    public bool IsAlive => currentHP > 0;

    [SerializeField] private int currentHP;

    private Animator animator;
    private bool died;

    void Awake()
    {   
        animator = gameObject.GetComponent<Animator>();
        PlayerSettings playerSettings = gameObject.GetComponent<PlayerSettings>();
        currentHP = playerSettings.maxHP;
        maxHP = playerSettings.maxHP;
        playerTransform = GetComponent<Transform>();
    }

    public void Increment(int incrementValue)
    {
        currentHP = Mathf.Clamp(currentHP + incrementValue, 0, maxHP);
    }

    public void Decrement(int decrementValue, float knockbackX, float knockbackY, Vector3 enemy_pos)
    {
        Debug.Log("decreasing health");
        currentHP = Mathf.Clamp(currentHP - decrementValue, 0, maxHP);
        if (currentHP == 0 && playerCanDie)
        {
            if (!died)
            {
                animator.SetTrigger("Death");
                Debug.Log("in death animation state: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));
                died = true;
            }
        }
        else
        {
            Debug.Log("hurt animation");
            animator.SetBool("Stun", true);
            
            Vector3 enemyDir = (playerTransform.position - enemy_pos).normalized;
            if (enemyDir.x < 0)
            {
                knockbackX = -knockbackX;
            }

            rBody.velocity = Vector3.zero;
            rBody.AddForce(new Vector3(knockbackX, knockbackY, 0), ForceMode2D.Impulse);

            player.stunTimer = stunTime;
            player.invincibleTimer = invincibleTime;


            // Tell animator that the player has been hurt
            //    animator.SetBool("Hurt", true);

            // Play SFX
            GameController.Instance.audioController.PlaySFX("Player Hit");

            
            Debug.Log("in stun animation state: " + animator.GetCurrentAnimatorStateInfo(0).IsName("stun"));
        }
    }

    void DeathAnimationEnded()
    {
        Destroy(gameObject);
    }

    void StunAnimationEnded()
    {
        animator.SetBool("Stun", false);
    }
}