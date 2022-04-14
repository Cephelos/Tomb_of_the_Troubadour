using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public RemovePlatforms removePlatforms;
    public SpawnEnemies spawnEnemies;

    public RaiseLava raiseLava;

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
        for(int i = 0; i < 1+Mathf.FloorToInt(intensity/2); i++) // Intensity goes up by 1 any time the audience finishes a vote. Use this to make the bad stuff get worse over time!
            spawnEnemies.Spawn(intensity);
    }

    public void RaiseLava() 
    {
        raiseLava.moveLava = true;
    }

    public void ResetRoom()
    {
        removePlatforms.ResetPlatforms();
        spawnEnemies.ResetEnemies();
    }
}
