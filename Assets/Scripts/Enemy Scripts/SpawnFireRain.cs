using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireRain : MonoBehaviour
{
     public List<Enemy> activeEnemies = new List<Enemy>();
    public Enemy firePrefab;
    public int numDefaultEnemies = 3;
    public float fadeTime = 2f; // time enemies take to fade in
    public Color enemyColor;

    [SerializeField]
    private float leftBound, rightBound, ceiling;
    [SerializeField]
    public void Spawn()
    {
        Enemy fire = Instantiate(firePrefab, transform);
        activeEnemies.Add(fire);
        Vector3 roomPos = Platformer.Mechanics.GameController.Instance.currentRoom.gameObject.transform.position;
        fire.transform.position = roomPos + new Vector3(Random.Range(leftBound, rightBound), ceiling, 0);

        Platformer.Mechanics.GameController.Instance.StartCoroutine(Summon(fire));
    }

    public IEnumerator Summon(Enemy fire)
    {

        fire.enabled = true;
        fire.collider.enabled = true;

        yield return null;
    }
}
