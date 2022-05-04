using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Health : MonoBehaviour
{
    public bool playerCanDie = false;
    public bool IsAlive => currentHP > 0;

    public int currentHP;
    [SerializeField] private int maxHP;
    [SerializeField] private float knockbackX = 5;
    [SerializeField] private float knockbackY = 5;
    [SerializeField] private float stunTime = 0.50f;
    [SerializeField] private float invincibleTime = 1f;

    private Animator animator;
    private HealthBar healthBar;
    private bool died;

    private Rigidbody2D rBody;
    private Transform playerTransform;
    private Platformer.Mechanics.PlayerMovement player;
    private Vector3 direction;
    private Weapon wep_script;
    private GameObject weapon;

    void Awake()
    {   
        animator = gameObject.GetComponent<Animator>();
        healthBar = GameObject.FindObjectOfType(typeof(HealthBar)) as HealthBar;
        PlayerSettings playerSettings = gameObject.GetComponent<PlayerSettings>();
        currentHP = playerSettings.maxHP;
        maxHP = playerSettings.maxHP;
        playerTransform = GetComponent<Transform>();
        rBody = GetComponent<Rigidbody2D>();
        player = GetComponent<Platformer.Mechanics.PlayerMovement>();
        weapon = gameObject.transform.GetChild(2).gameObject;
        wep_script = weapon.GetComponent<Weapon>();
    }

    public void Increment(int incrementValue)
    {
        currentHP = Mathf.Clamp(currentHP + incrementValue, 0, maxHP);
    }

    public void Decrement(int decrementValue, Vector3 enemy_pos, float knockbackX = 5, float knockbackY = 5)
    {
        currentHP = Mathf.Clamp(currentHP - decrementValue, 0, maxHP);
        if (gameObject.tag == "Player")
        {
            healthBar.DecreaseHealthBar(currentHP, maxHP);
        }
        if (currentHP == 0 && playerCanDie)
        {
            if (!died)
            {
                animator.SetTrigger("Death");
                died = true;

                // Play death sfx
                Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Player Die");
            }
        }
        else
        {
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
            // Play SFX
            Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Player Hit");
        }
    }

    void DeathAnimationEnded()
    {
        Debug.Log("the bitch died");
        Destroy(gameObject);
        Platformer.Mechanics.GameController.Instance.deathState();
    }

    void StunAnimationEnded()
    {
        Debug.Log("done being stunned");
        animator.SetBool("Stun", false);
    }
}