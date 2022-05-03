using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour // For storing variables related to enemies, such as health, attack power, etc.
{
    public Collider2D collider;
    public SpriteRenderer renderer;

    [SerializeField]
    private GameObject enemy;

    public bool invincible = false;

    [SerializeField]
    private float fallSpeed = 0, wobble = 100f;

    private float wobbleTimer;
    private bool wobbleDir = true;

    public int maxHP;
    int currentHP;

    void Start()
    {
        wobbleTimer = wobble;
        currentHP = maxHP;
    }

    void Update()
    {
        if(enemy.name.Contains("FireRainObject"))
        {

            wobbleTimer -= 1f * Time.deltaTime;
            if(wobbleTimer <= 0)
            {
                wobbleDir = !wobbleDir;
                wobbleTimer = wobble;
            }
            if(wobbleDir)
            {
                
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 5f, transform.position.y - 10), Time.deltaTime * fallSpeed);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 5f, transform.position.y - 10), Time.deltaTime * fallSpeed);
            }

            if(transform.position.y <= -18)
            {
                //enemy.GetComponent<Enemy>().Destruct();
                this.Destruct();
            }
        }
    }

    public void Destruct() // Removes enemy from spawnEnemies.activeEnemies and destroys it
    {
        Platformer.Mechanics.GameController.Instance.currentRoom.spawnEnemies.activeEnemies.Remove(this);
        Destroy(gameObject);

        // Play SFX
        Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Skeleton Die");
    }

    public void Decrement()
    {
        currentHP = Mathf.Clamp(currentHP - 1, 0, maxHP);
        if (currentHP == 0)
        {

            this.Destruct();
        }
    }
}
