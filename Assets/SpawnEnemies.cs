using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<Enemy> activeEnemies = new List<Enemy>();
    public Enemy enemyPrefab;
    public int numDefaultEnemies = 3;

    public Vector2 spawnLocUpperLeft; // These vector2s define the corners of the rectangle where enemies can spawn and move
    public Vector2 spawnLocLowerRight;
    public void Spawn(float intensity)
    {
        Enemy enemy = Instantiate(enemyPrefab, transform);
        activeEnemies.Add(enemy);
        enemy.transform.position = new Vector3(Random.Range(spawnLocUpperLeft.x, spawnLocLowerRight.x), Random.Range(spawnLocUpperLeft.y, spawnLocLowerRight.y), 0);
    }

    public void ResetEnemies()
    {
        while(activeEnemies.Count > 0)
        {
            Destroy(activeEnemies[0].gameObject);
            activeEnemies.RemoveAt(0);
        }
        int numEnemies = numDefaultEnemies+Random.Range(-1, 1);
        for(int i = 0; i < numEnemies; i++)
        {
            Spawn(1);
        }
    }
}
