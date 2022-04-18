using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Platformer.Mechanics.PlayerMovement player;
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
            w.Attack();
        }

        // Tell animator when the player begins their attack
        animator.SetBool("Attacking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift) && player.stunTimer == 0f)
        {
            Attack();
        }

        // Tell animator if the player is no longer attacking
        bool isAttacking = false;
        foreach(Weapon w in GetComponentsInChildren<Weapon>())
        {
            if (w.CanDamage())
                isAttacking = true;
        }
        if (!isAttacking)
            animator.SetBool("Attacking", false);
    }
}
