using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharGenVote : MonoBehaviour
{
    public VoteGenerator voteGenerator;
    public VoteEvery30Seconds eventVotes;
    public GameObject knightPrefab;
    public GameObject archerPrefab;
    public GameObject wizardPrefab;
    public GameObject spawnLoc;

    public Platformer.Mechanics.PlayerMovement playerMovement;
    public GameObject startDoor;

    private void Awake()
    {
        eventVotes.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMovement == null && voteGenerator.currentPoll.options.Count == 0) // If there's no active poll and no player spawned, create the first vote to select character class
            voteGenerator.CreateVote("What class is the player?", new string[3] { "Knight", "Archer", "Wizard" });

        if (voteGenerator.GetVoteResult() != "")
        {
            switch (voteGenerator.GetVoteResult())
            {
                case "Knight":
                    SpawnPlayerCharacter(0);
                    break;

                case "Archer":
                    SpawnPlayerCharacter(1);
                    break;

                case "Wizard":
                    SpawnPlayerCharacter(2);
                    break;

                case "Dash":
                    AssignSpecialAbility(0);
                    break;

                case "Double Jump":
                    AssignSpecialAbility(1);
                    break;

                case "Grapple":
                    AssignSpecialAbility(2);
                    break;

            }
        }
    }

    void SpawnPlayerCharacter(int whichCharacter) // Spawns in the character identified by the given integer: 0 = Knight, 1 = Archer, 2 = Wizard; then triggers the vote for ability selection
    {
        GameObject player = null;
        switch(whichCharacter)
        {
            case 0:
                player = Instantiate(knightPrefab);
                

                break;

            case 1:
                player = Instantiate(archerPrefab);

                break;

            case 2:
                player = Instantiate(wizardPrefab);

                break;
        }
        player.transform.position = spawnLoc.transform.position;
        playerMovement = player.GetComponent<Platformer.Mechanics.PlayerMovement>();
        Platformer.Mechanics.GameController.Instance.player = player.GetComponent<Platformer.Mechanics.PlayerMovement>();
        voteGenerator.ClosePoll();

        voteGenerator.CreateVote("What ability should they have?", new string[3] { "Dash", "Double Jump", "Grapple" });
    }

    void AssignSpecialAbility(int whichAbility) // Assigns the character one of the three special abilities: 0 = Dash, 1 = DblJump, 2 = Grapple; then closes the poll
    {
        switch(whichAbility)
        {
            case 0:
                playerMovement.SetAbility(whichAbility);
                break;

            case 1:
                playerMovement.SetAbility(whichAbility);
                break;

            case 2:
                playerMovement.SetAbility(whichAbility);
                break;
        }
        voteGenerator.ClosePoll();

        eventVotes.enabled = true;
        startDoor.SetActive(true);
    }
}
