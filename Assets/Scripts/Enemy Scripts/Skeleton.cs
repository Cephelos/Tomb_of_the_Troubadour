using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    private Health playerHealth;

    private enum State { Attack, Move, Idle };
    public float idleTime = 25f; // time skeleton idles for

    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [SerializeField] private int damage = 10;
    public int Damage { get { return damage; } set { damage = value; } }

    private Animator animator;
    private float timer;
    private State state;

    void Awake()
    {
        animator = GetComponent<Animator>();
        timer = idleTime;
        playerHealth = GameObject.FindObjectOfType(typeof(Health)) as Health;
    }

    void Update()
    {
        (bool, bool) leftPlayerDetection = DetectPlayer(-transform.right);
        (bool, bool) rightPlayerDetection = DetectPlayer(transform.right);
        bool edgeDetection = DetectEdge(transform.localScale.x*transform.right);
        // handle state changes
        if (state == State.Attack)
        {
            if (!leftPlayerDetection.Item2 && !rightPlayerDetection.Item2)
            {
                if (edgeDetection)
                {
                    animator.SetBool("Idle", true);
                    animator.SetBool("Attack", false);
                    state = State.Idle;
                }
                else
                {
                    animator.SetBool("Walk", true);
                    animator.SetBool("Attack", false);
                    state = State.Move;
                }
            }
        }
        else if (state == State.Move)
        {
            if (leftPlayerDetection.Item2 || rightPlayerDetection.Item2)
            {
                animator.SetBool("Attack", true);
                animator.SetBool("Walk", false);
                state = State.Attack;
            }
            else if (edgeDetection)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
                state = State.Idle;
            }
        }
        else if (state == State.Idle)
        {
            if (leftPlayerDetection.Item2 || rightPlayerDetection.Item2)
            {
                animator.SetBool("Attack", true);
                animator.SetBool("Idle", false);
                timer = idleTime;
                state = State.Attack;
            }
            else if (leftPlayerDetection.Item1 || rightPlayerDetection.Item1 && !edgeDetection)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);
                timer = idleTime;
                state = State.Move;
            }
            else if (!leftPlayerDetection.Item1 && !rightPlayerDetection.Item1 && timer <= 0.0f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Idle", false);
                ReverseDirection();
                timer = idleTime;
                state = State.Move;
            }
        }
        // handle actions based on state
        if (state == State.Move)
        {
            transform.position += transform.localScale.x * transform.right * Time.deltaTime * speed;
        }
        else if (state == State.Idle)
        {
            timer -= Time.deltaTime;
        }
    }

    (bool, bool) DetectPlayer(Vector3 direction)
    {
        // returns (true if player detected, true if close enough to attack player)
        Debug.DrawRay(transform.position, direction*10, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 10);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(direction.x))
                {
                    // reverse direction if needed
                    ReverseDirection();
                }
                if (-2 <= hit.distance && hit.distance <= 2)
                {
                    // close enough to player to attack
                    return (true, true);
                }
                else
                {
                    return (true, false);
                }
            }
            else if (hit.distance <= 1.5 && Mathf.Sign(transform.localScale.x) == Mathf.Sign(direction.x) && hit.collider.gameObject.tag == "Wall")
            {
                // detected wall
                if (state != State.Idle)
                {
                    animator.SetBool("Idle", true);
                    animator.SetBool("Attack", false);
                    animator.SetBool("Walk", false);
                    state = State.Idle;
                }
            }
        }
        return (false, false);
    }

    void ReverseDirection()
    {
        // flip skeleton object
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        // keep healthbar facing right
        Transform healthbarTransform = transform.Find("Canvas/HealthBarBackground/HealthBar");
        Vector3 healthbarScale = healthbarTransform.localScale;
        healthbarScale.x *= -1;
        healthbarTransform.localScale = healthbarScale;
    }

    bool DetectEdge(Vector3 direction)
    {
        Debug.DrawRay(transform.position, (-transform.up + direction/10)*2, Color.red);
        RaycastHit2D downHit = Physics2D.Raycast(transform.position, -transform.up + direction/10, 2);
        if (downHit.collider == null)
        {
            // did not hit ground so stop moving
            return true;
        }
        return false;
    }

    void AttackAnimationEnded()
    {
        if (playerHealth != null && state == State.Attack)
        {
            playerHealth.Decrement(damage, transform.position);
        }
    }
}