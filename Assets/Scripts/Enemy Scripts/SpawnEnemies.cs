using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<Enemy> activeEnemies = new List<Enemy>();
    public Enemy enemyPrefab;
    public int numDefaultEnemies = 3;
    public float fadeTime = 2f; // time enemies take to fade in
    public Color enemyColor;

    public Vector2 spawnLocLowerLeft; // These vector2s define the corners of the rectangle where enemies can spawn and move
    public Vector2 spawnLocUpperRight;
    public void Spawn(float intensity)
    {
        Enemy enemy = Instantiate(enemyPrefab, transform);
        activeEnemies.Add(enemy);
        Vector3 roomPos = Platformer.Mechanics.GameController.Instance.currentRoom.gameObject.transform.position;
        enemy.transform.position = roomPos + new Vector3(Random.Range(spawnLocLowerLeft.x, spawnLocUpperRight.x), Random.Range(spawnLocLowerLeft.y, spawnLocUpperRight.y), 0);
        enemy.collider.enabled = false;
        enemy.RBody.gravityScale = 0;
        
        Platformer.Mechanics.GameController.Instance.StartCoroutine(FadeIn(enemy, intensity));
    }

    public IEnumerator FadeIn(Enemy enemy, float intensity)
    {
        float timeRemaining = fadeTime;
        Color finalColor = Color.clear;
        while (timeRemaining > 0)
        {
            if (!enemy.gameObject.activeInHierarchy)
                yield break;
            float timeRatio = (fadeTime - timeRemaining) / fadeTime;


            if (enemy != null)
            {
                enemy.renderer.color = new Color(Mathf.Lerp(finalColor.r, enemyColor.r, timeRatio), Mathf.Lerp(finalColor.g, enemyColor.g, timeRatio), Mathf.Lerp(finalColor.b, enemyColor.b, timeRatio), Mathf.Lerp(finalColor.a, enemyColor.a, timeRatio));

            }
            else
                yield break;
            yield return new WaitForEndOfFrame();
            timeRemaining -= Time.deltaTime;
        }

        enemy.enabled = true;
        enemy.collider.enabled = true;
        enemy.RBody.gravityScale = 1;
        yield return null;
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
