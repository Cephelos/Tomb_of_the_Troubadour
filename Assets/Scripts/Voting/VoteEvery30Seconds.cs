using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteEvery30Seconds : MonoBehaviour
{
    public VoteGenerator voteGenerator;
    [SerializeField]private float voteTimer; // How long before the next vote
    public float intensity; // intensity of effects - higher means more enemies
    public float voteInterval; // How long until votes


    private void Awake()
    {
        voteTimer = voteInterval;
    }

    private void Update()
    {
        if(voteTimer <= 0f)
        {
            voteGenerator.CreateVote(voteGenerator.NextPoll());
            voteTimer = voteInterval;
        }    
        else
        {
            if(voteGenerator.GetVoteStatus() == 0)
                voteTimer -= Time.deltaTime;
        }

        if(voteGenerator.GetVoteResult() != "")
        {
            
            RoomManager room = Platformer.Mechanics.GameController.Instance.currentRoom;
            switch (voteGenerator.GetVoteResult())
            {
                case "More Enemies!":
                    room.SpawnEnemies(intensity);
                    break;

                case "Fire Rain!":
                    room.SpawnFireRain((int)Mathf.Floor(intensity)+3);
                    break;

                case "Raise the Lava!":

                    room.RaiseLava();
                    break;
            }
            voteGenerator.ClosePoll();

            intensity += 1f;
        }
    }
}
