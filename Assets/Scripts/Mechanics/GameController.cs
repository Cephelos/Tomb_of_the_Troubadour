using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlayerMovement player;
        public float baseGravity = 9.81f; // Base gravity value, resets to this when changing rooms
        public float basePlayerSpeed = 5f;
        public enum whichEnemiesToSpawn { All, Skeletons, Eyes }
        public whichEnemiesToSpawn whichEnemies = whichEnemiesToSpawn.All;
        public List<Enemy> enemyPrefabs;
        public PhysicsMaterial2D iceMat;

        public TilemapRenderer tiles;
        public Material defaultTiles;
        public Material frozenTiles;

        public ParticleSystem antigravParticles;
        public ParticleSystem saaanicParticles;

        public RoomManager currentRoom;
        public VoteGenerator votes;

        public AudioController audioController;

        public GameObject deathMessage;
        private bool dead = false;

        public int score = 0;

        // Sound FX
        public AudioClip walkSFX;
        public AudioClip jumpSFX;

        void OnEnable()
        {
            Instance = this;
            audioController = GetComponent<AudioController>();
            audioController.PlayMusic("BGM");
            votes = GetComponent<VoteGenerator>();
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
            if (dead && Input.GetKeyDown(KeyCode.Return)) mainMenu();
        }

        public void deathState()
        {
            deathMessage.SetActive(true);
            dead = true;
        }

        public void mainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}