using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VoteGenerator : MonoBehaviour
{
    public Poll currentPoll;
    public VotingPanel votePanel;

    public void CreateVote(string question, string[] options) // creates a poll and sets it to be the active poll; sends a warning to the console if there is already a poll active
    {
        CreateVote(new Poll(question, options));
    }

    public void CreateVote(Poll poll)
    {
        if (currentPoll != null)
            Debug.LogWarning("Creating a poll while poll " + currentPoll.question + " is already active!");
        currentPoll = poll;
        Debug.Log("Creating Poll " + currentPoll.question + "!");
        votePanel.Initialize(poll);
    }
        


    public int GetVoteStatus() // Returns 0 if no poll is currently active, 1 if there is an active poll but no votes have been cast, or 2 if one or more votes have been cast in the active poll
    {
        if (currentPoll == null)
            return 0;
        else if (currentPoll.options.Find(x => x.numVotes > 0) == null)
            return 1;
        else
            return 2;
    }

    public int GetVoteResult() // Returns the index number of the first option that has at least one vote, or -1 if none do
    {
        if (currentPoll == null)
            return -1;
        else
        {
            PollOption selected = currentPoll.options.Find(x => x.numVotes > 0);
            if (selected == null)
                return -1;
            else
                return currentPoll.options.IndexOf(selected);
        }
    }

    public void ClosePoll() // Closes the currently active poll
    {
        currentPoll = null;
        votePanel.ClearDisplays();
    }


    // Update is called once per frame
    void Update()
    {
        if(currentPoll != null)
            RecordVotesByKeyboard();
        PublishVoteResult();
    }

    public void PublishVoteResult() // IRL, the GameController is going to handle this
    {
        int voteResult = GetVoteResult();
        if (voteResult >= 0)
        {
            Debug.Log("Vote Result : " + currentPoll.options[voteResult].optionName);
            ClosePoll();
        }
    }

    public void RecordVotesByKeyboard() // Adds a vote to the corresponding poll option if the player has hit any keys; sends a warning to the console if someone tries to vote for something invalid
    {
        for (int number = 1; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()))

            {
                if (currentPoll.options.Count >= number)
                    currentPoll.RecordVote(number);
                else
                    Debug.LogWarning("Illegal vote attempted! Current poll doesn't have " + number +" options!");
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(VoteGenerator))]
public class VoteGeneratorEditor : Editor // Lets you generate polls using the inspector GUI
{
    public Poll pollUnderConstruction  = new Poll("New Poll", new string[0]);
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VoteGenerator vote = target as VoteGenerator;
        if(pollUnderConstruction != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Poll Question:");
            pollUnderConstruction.question = GUILayout.TextField(pollUnderConstruction.question);
            GUILayout.EndHorizontal();

            for(int i = 0; i < pollUnderConstruction.options.Count; i++)
            {
                PollOption option = pollUnderConstruction.options[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label("Option " + (i+1) + ":");
                option.optionName = GUILayout.TextField(option.optionName);
                GUILayout.EndHorizontal();
                if(GUILayout.Button("Remove Option"))
                {
                    pollUnderConstruction.options.RemoveAt(i);
                    i--;
                }
            }
            if (GUILayout.Button("Add New Option"))
                pollUnderConstruction.options.Add(new PollOption("New Option"));

            if (GUILayout.Button("Open Poll"))
            {
                vote.CreateVote(pollUnderConstruction);
                pollUnderConstruction = new Poll("New Poll", new string[0]);
            }

        }
    }
}
#endif

public class Poll // Represents a question on which voiting will take place; options are voted for by inputing their index +1 (so voting for option 0 in the list would require pressing '1')
{
    public string question;
    public List<PollOption> options;

    public void RecordVote(int whichOption)
    {
        options[whichOption-1].numVotes++;
        Debug.Log("Vote Recorded for option " + whichOption + " in poll \'" + question +"\'!");
    }

    public Poll(string _question, string[] optionNames)
    {
        question = _question;
        options = new List<PollOption>();
        foreach(string option in optionNames)
        {
            options.Add(new PollOption(option));
        }
    }
}

public class PollOption
{
    public string optionName; // Describes the option
    public int numVotes; // Number of votes for this option so far

    public PollOption(string _optionName)
    {
        optionName = _optionName;
    }
}
