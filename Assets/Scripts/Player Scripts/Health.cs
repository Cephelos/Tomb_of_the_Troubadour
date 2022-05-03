using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool playerCanDie = false;
    public bool IsAlive => currentHP > 0;

    [SerializeField] private int currentHP;
    private int maxHP;

    private Animator animator;
    private bool died;

    void Awake()
    {   
        animator = gameObject.GetComponent<Animator>();
        PlayerSettings playerSettings = gameObject.GetComponent<PlayerSettings>();
        currentHP = playerSettings.maxHP;
        maxHP = playerSettings.maxHP;
    }

    public void Increment(int incrementValue)
    {
        currentHP = Mathf.Clamp(currentHP + incrementValue, 0, maxHP);
    }

    public void Decrement(int decrementValue)
    {
        Debug.Log(currentHP);
        currentHP = Mathf.Clamp(currentHP - decrementValue, 0, maxHP);
        if (currentHP == 0 && playerCanDie)
        {
            if (!died)
            {
                animator.SetTrigger("Death");
                died = true;
            }
        }
        else
        {
            animator.SetBool("Stun", true);
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