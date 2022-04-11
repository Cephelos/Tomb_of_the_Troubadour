using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VotingPanel : MonoBehaviour
{
    public GameObject pollOptionDisplayPrefab;
    public List<GameObject> pollQuestionDisplays = new List<GameObject>();
    public TextMeshProUGUI pollQuestionText;
    public Image image;
    public Color baseColor;


    private void Awake()
    {
        baseColor = image.color;
        ClearDisplays();
    }
    public void Initialize(Poll poll)
    {
        ClearDisplays();
        pollQuestionText.text = poll.question;
        for(int i = 0; i < poll.options.Count; i++)
        {
            PollOption option = poll.options[i];
            GameObject newOptionDisplay = Instantiate(pollOptionDisplayPrefab, transform);
            TextMeshProUGUI displayText = newOptionDisplay.GetComponentInChildren<TextMeshProUGUI>();
            displayText.text = (i+1) + ": " +option.optionName;
            pollQuestionDisplays.Add(newOptionDisplay);
        }
        image.color = baseColor;
    }

    public void ClearDisplays()
    {
        pollQuestionText.text = "";
        while(pollQuestionDisplays.Count > 0)
        {
            Destroy(pollQuestionDisplays[0]);
            pollQuestionDisplays.RemoveAt(0);
        }
        image.color = Color.clear;
    }
}
