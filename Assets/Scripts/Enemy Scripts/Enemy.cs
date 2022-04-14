using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour // For storing variables related to enemies, such as health, attack power, etc.
{
    public Collider2D collider;
    public SpriteRenderer renderer;
    public FollowPath followPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakePath(Vector2 lowerLeft, Vector2 upperRight, int nodes)
    {
        for(int i = 0; i < nodes; i++)
        {
            followPath.AddNode(new Vector2(Random.Range(lowerLeft.x, upperRight.x), Random.Range(lowerLeft.y, upperRight.y)));
        }
    }

    public void Destruct() // Removes enemy from spawnEnemies.activeEnemies and destroys it
    {
        Platformer.Mechanics.GameController.Instance.currentRoom.spawnEnemies.activeEnemies.Remove(this);
        Destroy(gameObject);
    }
}
