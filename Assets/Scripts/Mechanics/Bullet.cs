
using System.Collections;

using System.Collections.Generic;

using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float speed = 20;

    void Update()
    {
        transform.Translate((new Vector2(1,0) * speed * Time.deltaTime));

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall") || collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}