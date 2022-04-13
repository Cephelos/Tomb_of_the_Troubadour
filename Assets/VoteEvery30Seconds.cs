using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteEvery30Seconds : MonoBehaviour
{
    public VoteGenerator voteGenerator;
    [SerializeField]private float voteTimer; // How long before the next vote
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
    }
}
