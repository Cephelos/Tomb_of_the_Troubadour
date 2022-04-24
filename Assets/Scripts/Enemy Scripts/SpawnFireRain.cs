using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireRain : MonoBehaviour
{
     public List<Enemy> activeFire = new List<Enemy>();
    public Enemy firePrefab;


    [SerializeField]
    private float leftBound, rightBound, ceiling;
    [SerializeField]
    public void Spawn()
    {
        Enemy fire = Instantiate(firePrefab, transform);
        activeFire.Add(fire);
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

    public void ResetFire()
    {

        while(activeFire.Count > 0)
        {
            Destroy(activeFire[0].gameObject);
            activeFire.RemoveAt(0);
        }
    }
}
