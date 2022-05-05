using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flyTime = 50f; // time eye flies in one direction for

    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    private Animator animator;

    [SerializeField] private float timer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        timer = flyTime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = NextDirection();
        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(direction.x))
        {
            // reverse direction if needed
            ReverseDirection();
        }
        //Debug.Log(direction);
        transform.position += direction * Time.deltaTime * speed;
    }

    Vector3 NextDirection()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        if (colliders != null && colliders.Length > 0)
        {
            // fly in opposite direction of all object about to hit
            Vector3 vectorFromObjects = new Vector3(0.0f, 0.0f, 0.0f);
            foreach (Collider2D collider in colliders)
            {
                Transform objectTransform = collider.gameObject.GetComponent<Transform>();
                vectorFromObjects += transform.position - objectTransform.position;
            }
            return vectorFromObjects.normalized;
        }
        else if (timer <= 0.0f)
        {
            // fly in a new random direction after timer has elapsed
            Vector3 direction = Random.insideUnitCircle.normalized;
            timer = flyTime;
            return direction;
        }
        else
        {
            timer -= Time.deltaTime;
            return transform.localScale.normalized;
        }
    }

    void ReverseDirection()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}