using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RemovePlatforms removePlatforms;
    public SpawnEnemies spawnEnemies;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemovePlatforms(float intensity)
    {
        removePlatforms.RemoveRandomPlatform(intensity);
    }

    public void SpawnEnemies(float intensity)
    {
        spawnEnemies.Spawn(intensity);
    }

    public void ResetRoom()
    {
        removePlatforms.ResetPlatforms();
        spawnEnemies.ResetEnemies();
    }
}
