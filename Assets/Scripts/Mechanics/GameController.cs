using Platformer.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        public PhysicsMaterial2D iceMat;

        public TilemapRenderer tiles;
        public Material defaultTiles;
        public Material frozenTiles;

        public ParticleSystem antigravParticles;
        public ParticleSystem saaanicParticles;

        public RoomManager currentRoom;

        public AudioController audioController;

        // Sound FX
        public AudioClip walkSFX;
        public AudioClip jumpSFX;

        void OnEnable()
        {
            Instance = this;
            audioController = GetComponent<AudioController>();
            audioController.PlayMusic("BGM");
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }
    }
}