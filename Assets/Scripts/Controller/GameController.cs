using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private enum GameState { InitialVote, RoomOne, RoomTwo, RoomThree };
    private GameState gameState;
    private VoteGenerator voteGenerator;
    private List<Room> rooms;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        this.rooms = new List<Room>()
        {
            RoomGenerator.GenerateNewRoom(0), // Initial vote room where player can practice/move about
            RoomGenerator.GenerateNewRoom(1),
            RoomGenerator.GenerateNewRoom(2),
            RoomGenerator.GenerateNewRoom(3)
        };
        this.player = new Player();
        this.gameState = InitialVote;
    }

    // Update is called once per frame
    void Update()
    {
        int playerResult = 0; // 0: no state change, 1: player died, 2: player went through door
        switch (gameState)
        {
            case GameState.InitialVote:
                playerResult = this.rooms[0].Update(this.player);
                this.HandleVoting();
                break;
            case GameState.RoomOne:
                playerResult = this.rooms[1].Update(this.player);
                if (playerResult == 2)
                {
                    this.GameState = GameState.RoomTwo;
                };
                break;
            case GameState.RoomTwo:
                playerResult = this.rooms[2].Update(this.player);
                if (playerResult == 2)
                {
                    this.GameState = GameState.RoomThree;
                };
                break;
            case GameState.RoomThree:
                playerResult = this.rooms[3].Update(this.player);
                if (playerResult == 2)
                {
                    this.GameState = GameState.RoomOne;
                };
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
                this.voteGenerator.CreateVote("Player Movement Speed?", { "1x", "1.5x", "2x" }); // hard coded poll for now
                break;
            case 1:
                // poll active but not finished yet
                this.voteGenerator.Update();
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
        this.rooms[1].ResetRoom();
        this.rooms[2].ResetRoom();
        this.rooms[3].ResetRoom();
        this.player.ResetPlayer();

        // hard coded for now
        switch (initialVoteResult)
        {
            case "1x":
                this.player.SetPlayerSpeed(1);
                break;
            case "1.5x":
                this.player.SetPlayerSpeed(1.5);
                break;
            case "2x":
                this.player.SetPlayerSpeed(2);
                break;
            default:
                break;
        }

        // switch to room one
        this.gameState = GameState.RoomOne;
    }
}