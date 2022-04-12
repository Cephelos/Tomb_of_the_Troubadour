using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class GameController : MonoBehaviour
{
    private enum GameState { InitialVote, Room};
    private GameState gameState;
    private VoteGenerator voteGenerator;
    [SerializeField] private List<GameObject> rooms;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        
        gameState = GameState.InitialVote;
    }

    // Update is called once per frame
    void Update()
    {
        int playerResult = 0; // 0: no state change, 1: player died, 2: player went through door
        switch (gameState)
        {
            case GameState.InitialVote:

                this.HandleVoting();
                break;
            case GameState.Room:

                break;
            default:
                break;
        }
        if (playerResult == 1)
        {
            this.gameState = GameState.InitialVote;
        };
    }

    private void HandleVoting()
    {
        switch (this.voteGenerator.GetVoteStatus())
        {
            case 0:
                // poll not active so create one
                string[] choices = {"1x", "1.5x", "2x" };
                this.voteGenerator.CreateVote("Player Movement Speed?", choices); // hard coded poll for now
                break;
            case 1:
                // poll active but not finished yet
                this.voteGenerator.UpdateVotes();
                break;
            case 2:
                // poll finished
                this.StartNewGame(this.voteGenerator.GetVoteResult());
                this.voteGenerator.ClosePoll();
                break;
            default:
                break;
        }
    }

    private void StartNewGame(string initialVoteResult)
    {
        // reset rooms, reset player
        // this.rooms[1].ResetRoom();
        // this.rooms[2].ResetRoom();
        // this.rooms[3].ResetRoom();
        // this.player.ResetPlayer();

        // hard coded for now
        switch (initialVoteResult)
        {
            case "1x":
                
                break;
            case "1.5x":
                this.player.GetComponent<PlayerMovement>().speed *= 1.5f;
                break;
            case "2x":
                this.player.GetComponent<PlayerMovement>().speed *= 2f;
                break;
            default:
                break;
        }

        // switch to room one
        this.gameState = GameState.Room;
    }
}