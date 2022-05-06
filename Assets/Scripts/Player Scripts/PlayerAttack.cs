using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Platformer.Mechanics.PlayerMovement player;
    [SerializeField] private float bowCooldown = 0.75f;
    private float bowTimer = 0f;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Platformer.Mechanics.PlayerMovement>();
    }

    void Attack()
    {
        foreach (Weapon w in GetComponentsInChildren<Weapon>())
        {
            w.Attack(player);
        }

        // Tell animator when the player begins their attack
        animator.SetBool("Attacking", true);
    }

    void TickTimers()
    {
        if (bowTimer > 0)
            bowTimer -= Time.deltaTime;
        if (bowTimer < 0)
            bowTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {   
        TickTimers();


        if (player.CanAct() && Input.GetKeyDown(KeyCode.RightShift))
        {
            if(bowTimer == 0)
            {
                Attack();
                bowTimer = bowCooldown;
            }
            
        }

        // Tell animator if the player is no longer attacking
        bool isAttacking = false;
        foreach(Weapon w in GetComponentsInChildren<Weapon>())
        {
            if (w.timeSinceAttack < w.attackDuriation)
                isAttacking = true;
        }
        if (!isAttacking)
            animator.SetBool("Attacking", false);
    }
}
