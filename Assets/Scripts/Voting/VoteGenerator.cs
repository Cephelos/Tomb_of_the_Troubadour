using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


/// HOW TO POKEMON GO TO THE POLLS:
/// 1. Have the VoteGenerator's GameObject selected in the editor
/// 2. Name your poll using the field under '----FOR TESTING----' in the inspector
/// 3. Add options to the poll
/// 3. ????
/// 4. Profit


public class VoteGenerator : MonoBehaviour
{
    public Poll currentPoll;
    public VotingPanel votePanel;
    public List<Poll> polls;

    public List<string> pollOptionNames = new List<string>();
    public List<string> availablePollOptionNames = new List<string>();


    private void Start()
    {
        ResetAvailablePollOptions();
    }

    void ResetAvailablePollOptions()
    {
        availablePollOptionNames.Clear();
        availablePollOptionNames.AddRange(pollOptionNames);
        Platformer.Mechanics.GameController gameController = Platformer.Mechanics.GameController.Instance;
        if (gameController.currentRoom.IsRoomFrozen())
            availablePollOptionNames.Remove("Freeze Floors!");

        if (Mathf.Abs(Physics2D.gravity.y) < gameController.baseGravity)
            availablePollOptionNames.Remove("Decrease Gravity!");

        if(gameController.player != null)
        {
            if (gameController.player.speed > gameController.basePlayerSpeed)
                availablePollOptionNames.Remove("Speed up Player!");

        }
    }
    public void CreateVote(string question, string[] options) // creates a poll and sets it to be the active poll; sends a warning to the console if there is already a poll active
    {
        CreateVote(new Poll(question, options));
    }

    public void CreateVote(Poll poll)
    {
        if (currentPoll != null && currentPoll.options != null && currentPoll.options.Count != 0)
            Debug.LogWarning("Creating a poll while poll " + currentPoll.question + " is already active!");
        currentPoll = poll;
        Debug.Log("Creating Poll " + currentPoll.question + "!");
        votePanel.Initialize(poll);
    }

    public void CreateTwoOptionVote()
    {
        int firstSelectionID = Random.Range(0,availablePollOptionNames.Count);
        int secondSelectionID = firstSelectionID + Random.Range(1, availablePollOptionNames.Count - 1);
        string firstOption = availablePollOptionNames[firstSelectionID];
        if (secondSelectionID > availablePollOptionNames.Count - 1)
            secondSelectionID -= availablePollOptionNames.Count;
        string secondOption = availablePollOptionNames[secondSelectionID]; // this will be important later
        CreateVote(new Poll("What should happen next?", new string[2] { firstOption, secondOption }));

        // Prevent repeated poll options
        ResetAvailablePollOptions(); 
        
        // Only remove both options if we have at least 4 options to begin with
        if(availablePollOptionNames.Count > 3)
        {

            availablePollOptionNames.Remove(firstOption);
            availablePollOptionNames.Remove(secondOption);
        }
    }
        
    public Poll NextPoll() // Gets the next poll on the list and returns it, then moves it to the back of the list
    {
        Poll nextPoll = polls[0];
        polls.RemoveAt(0);
        polls.Add(nextPoll);
        return nextPoll.Clone();

    }

    public int GetVoteStatus() // Returns 0 if no poll is currently active, 1 if there is an active poll but no votes have been cast, or 2 if one or more votes have been cast in the active poll
    {
        if (currentPoll == null || currentPoll.options == null)
            return 0;
        else if (currentPoll.options.Count == 0)
            return 0;
        else if (currentPoll.options.Find(x => x.numVotes > 0) == null)
            return 1;
        else
            return 2;
    }

    public string GetVoteResult() // Returns the index number of the first option that has at least one vote, or -1 if none do
    {
        if (currentPoll == null || currentPoll.options == null)
            return "";
        else if (currentPoll.options.Count == 0)
            return "";
        else
        {
            PollOption selected = currentPoll.options.Find(x => x.numVotes > 0);
            if (selected == null)
                return "";
            else
                return selected.optionName;
        }
    }

    public int GetVoteResultInt() // Returns the index number of the first option that has at least one vote, or -1 if none do
    {
        if (currentPoll == null || currentPoll.options == null)
            return -1;
        else if (currentPoll.options.Count == 0)
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

    private void Update()
    {
        UpdateVotes();
    }

    // Update is called once per frame
    public void UpdateVotes()
    {
        if(currentPoll != null && currentPoll.options != null)
            RecordVotesByKeyboard();
       //PublishVoteResult();
    }

    public void PublishVoteResult() // IRL, the GameController is going to handle this - this is just used for testing
    {
        int voteResult = GetVoteResultInt();
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
public class VoteGeneratorEditor : Editor // Lets you generate polls using the inspector GUI (for testing purposes)
{
    public Poll pollUnderConstruction  = new Poll("New Poll", new string[0]);
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        VoteGenerator vote = target as VoteGenerator;
        /*
            GUILayout.Label("----FOR TESTING----");
            if (pollUnderConstruction != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Poll Question:");
                pollUnderConstruction.question = GUILayout.TextField(pollUnderConstruction.question);
                GUILayout.EndHorizontal();

                for (int i = 0; i < pollUnderConstruction.options.Count; i++)
                {
                    PollOption option = pollUnderConstruction.options[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Option " + (i + 1) + ":");
                    option.optionName = GUILayout.TextField(option.optionName);
                    if (GUILayout.Button("Remove Option"))
                    {
                        pollUnderConstruction.options.RemoveAt(i);
                        i--;
                    }
                    GUILayout.EndHorizontal();

                }
                if (GUILayout.Button("Add New Option"))
                    pollUnderConstruction.options.Add(new PollOption("New Option"));
            if (Application.IsPlaying(this))
            {
                if (GUILayout.Button("Open Poll"))
                {
                    vote.CreateVote(pollUnderConstruction);
                    pollUnderConstruction = new Poll("New Poll", new string[0]);
                }
                

            }
            else
            {
                if (GUILayout.Button("Add Poll"))
                {
                    vote.polls.Add(pollUnderConstruction);
                    pollUnderConstruction = new Poll("New Poll", new string[0]);
                }
            }

        }
        

        GUILayout.Label("Add Poll Options Here:");

        for (int i = 0; i < vote.pollOptionNames.Count; i++)
        {
            string optionName = vote.pollOptionNames[i];
            GUILayout.BeginHorizontal();
            GUILayout.Label("Option " + (i + 1) + ":");
            optionName = GUILayout.TextField(optionName);
            if (GUILayout.Button("Remove Option"))
            {
                vote.pollOptionNames.RemoveAt(i);
                i--;
            }
            GUILayout.EndHorizontal();

        }
        if (GUILayout.Button("Add New Option"))
            vote.pollOptions.Add(new PollOption("New Option"));
        */

        if (GUILayout.Button("Close Poll"))
        {
            vote.ClosePoll();
        }
    }
}
#endif

[System.Serializable]
public class Poll // Represents a question on which voiting will take place; options are voted for by inputing their index +1 (so voting for option 0 in the list would require pressing '1')
{
    public string question;
    public List<PollOption> options = new List<PollOption>();

    public void RecordVote(int whichOption)
    {
        options[whichOption-1].numVotes++;
        Debug.Log("Vote Recorded for option " + whichOption + " in poll \'" + question +"\'!");
    }

    public Poll(string _question, string[] optionNames)
    {
        question = _question;
        foreach(string option in optionNames)
        {
            options.Add(new PollOption(option));
        }
    }

    public Poll Clone() // Clones a poll - use this when instantiating new polls to avoid corrupting the old poll instance
    {
        List<string> optionNames = new List<string>();
        foreach (PollOption option in options)
            optionNames.Add(option.optionName);
        Poll clone = new Poll(question, optionNames.ToArray());

        return clone;
    }
}

[System.Serializable]
public class PollOption
{
    public string optionName; // Describes the option
    public int numVotes; // Number of votes for this option so far

    public PollOption(string _optionName)
    {
        optionName = _optionName;
    }
}
