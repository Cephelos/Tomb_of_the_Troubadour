using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{

    public TMPro.TextMeshProUGUI text;
    
    public void DisplayScore((int, int) scores)
    {
        text.text = "Rooms Cleared: " + scores.Item1 + "\n" + "Enemies Killed: " + scores.Item2;
    }
}
