using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomManager : MonoBehaviour
{

    public SpawnEnemies spawnEnemies;

    public RaiseLava raiseLava;

    public SpawnFireRain spawnFireRain;

    public ParticleSystem antigravParticles;

    public AudioSource audioSource;
    public GameObject[] spawnLocations;
    private int fireCount;
    private int waveCounter = 0;

    public int numWaves = 1;


    private void Start()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(AllEnemiesDead() && waveCounter < numWaves && Platformer.Mechanics.GameController.Instance.currentRoom.gameObject.name != "Room 0")
        {
            waveCounter += 1;
            Platformer.Mechanics.GameController.Instance.currentRoom.gameObject.transform.Find("Enemies").GetComponent<SpawnEnemies>().ResetEnemies();

        }
    }

    public AudioController AudioController { get { return Platformer.Mechanics.GameController.Instance.audioController; } }

    public bool AllEnemiesDead()
    {
        return spawnEnemies.activeEnemies.Count == 0;
    }


    public void SpawnEnemies(float intensity)
    {

        for(int i = 0; i < 1+Mathf.FloorToInt(intensity/2); i++) // Intensity goes up by 1 any time the audience finishes a vote. Use this to make the bad stuff get worse over time!
            {
            int whichEnemies = (int)Platformer.Mechanics.GameController.Instance.whichEnemies;
            spawnEnemies.Spawn(intensity, Random.Range(whichEnemies != 2 ? 0 : 1, whichEnemies != 1 ? 2 : 1));
        }
    }

    public void RaiseLava() 
    {
        raiseLava.moveLava = true;
        AudioController.PlaySFX("Raise Lava");
    }

    public void SpawnFireRain(int fireCount)
    {
        for(int i = 0; i < fireCount; i++) 
            spawnFireRain.Spawn();
        AudioController.PlaySFX("Fire Rain");
    }

    public void DecreaseGravity() // Cuts gravity in half
    {
        Physics2D.gravity -= Physics2D.gravity / 2;
        antigravParticles = Instantiate(Platformer.Mechanics.GameController.Instance.antigravParticles, transform, false);
        AudioController.PlaySFX("Dec Grav", false, true);
    }

    public bool IsRoomFrozen()
    {

        Platformer.Mechanics.GameController gameController = Platformer.Mechanics.GameController.Instance;
        if (gameController.tiles.sharedMaterial == gameController.frozenTiles)
            return true;
        else
            return false;
    }

    public void FreezeFloors() // Nasty
    {
        Platformer.Mechanics.GameController gameController = Platformer.Mechanics.GameController.Instance;
        gameController.player.GetComponent<Platformer.Mechanics.PlayerMovement>().ToggleIcePhysics(true);
        gameController.tiles.sharedMaterial = gameController.frozenTiles;
        AudioController.PlaySFX("Freeze Platforms");
    }

    public void SpeedUpPlayer()
    {
        Platformer.Mechanics.GameController.Instance.player.GetComponent<Platformer.Mechanics.PlayerMovement>().BoostSpeed(true);
        Platformer.Mechanics.GameController.Instance.audioController.PlaySFX("Speed Boost Fire");
        AudioController.PlaySFX("Speed Boost", false, true);
    }

    public void ResetRoom() // Called when a room is entered
    {
        //removePlatforms.ResetPlatforms();
        waveCounter = 0;
        spawnEnemies.InitSpawner();
        spawnEnemies.ResetEnemies();
        raiseLava.ResetLava();
        spawnFireRain.ResetFire();

        Platformer.Mechanics.GameController gameController = Platformer.Mechanics.GameController.Instance;
        Physics2D.gravity = Vector2.down * gameController.baseGravity;
        if (antigravParticles != null)
        {
            Destroy(antigravParticles.gameObject);
            antigravParticles = null;
        }

        gameController.player.GetComponent<Platformer.Mechanics.PlayerMovement>().ToggleIcePhysics(false);
        gameController.tiles.sharedMaterial = gameController.defaultTiles;

        gameController.player.GetComponent<Platformer.Mechanics.PlayerMovement>().BoostSpeed(false);
        gameController.audioController.StopSFX("Dec Grav");
        gameController.audioController.StopSFX("Speed Boost");
    }


}

#if UNITY_EDITOR
[CustomEditor(typeof(RoomManager))]
public class RoomEditor : Editor
{
    public int eventIntensity;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoomManager roomManager = (RoomManager)target;

        GUILayout.Label("---FOR TESTING---");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Event Intensity: ");
        eventIntensity = EditorGUILayout.IntField(eventIntensity);

        GUILayout.EndHorizontal();
        if (GUILayout.Button("Fire Rain"))
            roomManager.SpawnFireRain(eventIntensity);
        if (GUILayout.Button("Spawn Enemies"))
            roomManager.SpawnEnemies(eventIntensity);
        if (GUILayout.Button("Raise Lava"))
            roomManager.RaiseLava();
        if (GUILayout.Button("Decrease Gravity"))
            roomManager.DecreaseGravity();
        if (GUILayout.Button("Freeze Floors"))
            roomManager.FreezeFloors();
        if (GUILayout.Button("Speed up Player"))
            roomManager.SpeedUpPlayer();
    }
}
#endif