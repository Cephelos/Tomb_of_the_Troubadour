using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<Enemy> activeEnemies = new List<Enemy>();
    public int numDefaultEnemies = 4;
    public float fadeTime = 2f; // time enemies take to fade in
    public Color enemyColor;

    public Vector2 spawnLocLowerLeft; // These vector2s define the corners of the rectangle where enemies can spawn and move
    public Vector2 spawnLocUpperRight;

    private GameObject[] enemyPosList;

    void Start()
    {

    }

    public void InitSpawner() // Gets the list of enemy spawners for the current room (have to do this after switching rooms)
    {
        enemyPosList = Platformer.Mechanics.GameController.Instance.currentRoom.gameObject.GetComponent<RoomManager>().spawnLocations;
    }

    public void Spawn(float intensity, int whichEnemy = 0)
    {
        Enemy enemy = Instantiate(Platformer.Mechanics.GameController.Instance.enemyPrefabs[whichEnemy], transform);
        activeEnemies.Add(enemy);
        Vector3 spawnLoc = enemyPosList[Random.Range(0, enemyPosList.Length-1)].transform.position;
        enemy.transform.position = new Vector3(spawnLoc.x + Random.Range(-2, 2), spawnLoc.y, spawnLoc.z);
        enemy.collider.enabled = false;
        enemy.GetComponent<Rigidbody2D>().isKinematic = true;
        Platformer.Mechanics.GameController.Instance.StartCoroutine(FadeIn(enemy, intensity));

        // Play spawn SFX (if not playing)
        AudioController audioController = Platformer.Mechanics.GameController.Instance.audioController;
        if(!audioController.IsPlayingSFX("Skeleton Spawn"))
        {
            audioController.PlaySFX("Skeleton Spawn");
        }
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
        enemy.GetComponent<Rigidbody2D>().isKinematic = false;
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
            int whichEnemies = (int)Platformer.Mechanics.GameController.Instance.whichEnemies;
            Spawn(1, Random.Range(whichEnemies != 2 ? 0 : 1, whichEnemies != 1 ? 2 : 1));
        }
    }
}
