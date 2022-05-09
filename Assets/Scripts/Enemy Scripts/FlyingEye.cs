using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    private enum State { Attack, Peace };

    public float flyTime; // time eye flies in one direction for

    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    [SerializeField] private int damage = 10;
    public int Damage { get { return damage; } set { damage = value; } }

    private Animator animator;
    private EnemyHealth health;
    private Transform playerTransform;
    private Health playerHealth;
    private Vector3 direction;
    private State state;

    [SerializeField] private float timer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<EnemyHealth>();
        var player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        playerHealth = player.GetComponent<Health>();
        timer = flyTime;
        direction = Random.insideUnitCircle.normalized;
        if (Mathf.Sign(direction.x) != 1f)
        {
            ReverseDirection();
        }
        state = State.Peace;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Peace)
        {
            if (playerTransform != null && health.IsHurt)
            {
                gameObject.GetComponent<Renderer>().material.color = new Color32(255, 17, 0, 255);
                state = State.Attack;
            }
        }
        else if (state == State.Attack)
        {
            if (playerTransform == null)
            {
                // player died
                gameObject.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
                state = State.Peace;
            }
        }

        Vector3 newDirection = Vector3.zero;
        if (state == State.Peace)
        {
            newDirection = NextDirection();
        }
        else
        {
            newDirection = MoveTowardsPlayer();
        }
        if (Mathf.Sign(newDirection.x) != Mathf.Sign(direction.x))
        {
            // reverse direction if needed
            ReverseDirection();
        }
        direction = newDirection;
        transform.position += direction * Time.deltaTime * speed;
    }

    Vector3 NextDirection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.5f);
        if (hit.collider != null)
        {
            // fly in opposite direction of object about to hit
            Vector3 vectorFromObject = transform.position - (Vector3)hit.point;
            return vectorFromObject.normalized;
        }
        else if (timer <= 0.0f)
        {
            // fly in a new random direction after timer has elapsed
            timer = flyTime;
            return Random.insideUnitCircle.normalized;
        }
        else
        {
            timer -= Time.deltaTime;
            return direction;
        }
    }

    Vector3 MoveTowardsPlayer()
    {
        Vector3 playerDirection = playerTransform.position - transform.position;
        playerDirection.z = 0;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, 1.5f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                // attack player
                animator.SetBool("Attack", true);
                return Vector3.zero;
            }
            else if (hit.collider.gameObject.tag == "Lava")
            {
                // stop moving since about to hit lava
                return Vector3.zero;
            }
            else
            {
                animator.SetBool("Attack", false);
                // move along obstacle until clear of it
                if (hit.normal == Vector2.down || hit.normal == Vector2.up)
                {
                    // move horizontally
                    return new Vector3(5.0f, 0.0f, 0.0f);
                }
                else if (hit.normal == Vector2.left || hit.normal == Vector2.right)
                {
                    // move vertically
                    if (playerTransform.position.y > transform.position.y)
                    {
                        // move up
                        return new Vector3(0.0f, 5.0f, 0.0f);
                    }
                    else
                    {
                        // move down
                        return new Vector3(0.0f, -5.0f, 0.0f);
                    }
                }
                return Vector3.zero;
            }
        }
        else
        {
            animator.SetBool("Attack", false);
            // just move towards player
            return playerDirection;
        }
    }

    void ReverseDirection()
    {
        // flip flying eye object
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
        // keep healthbar facing right
        Transform healthbarTransform = transform.Find("Canvas/HealthBarBackground/HealthBar");
        Vector3 healthbarScale = healthbarTransform.localScale;
        healthbarScale.x *= -1;
        healthbarTransform.localScale = healthbarScale;
    }

    void AttackAnimationEnded()
    {
        if (playerHealth != null && state == State.Attack)
        {
            playerHealth.Decrement(damage, transform.position);
        }
    }
}