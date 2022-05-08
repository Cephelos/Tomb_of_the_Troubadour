using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;
    private Color32 green;
    private Color32 yellow;
    private Color32 orange;
    private Color32 red;

    void Awake()
    {   
        healthBar = gameObject.GetComponent<Image>();
        green = new Color32(0, 128, 0, 255);
        yellow = new Color32(255, 255, 0, 255);
        orange = new Color32(255, 140, 0, 255);
        red = new Color32(255, 0, 0, 255);
    }

    public void DecreaseHealthBar(int currentHP, int maxHP)
    {
        // set fill amount
        float healthPercent = (float)currentHP/maxHP;
        if (healthPercent != healthBar.fillAmount)
        {
            healthBar.fillAmount = healthPercent;
        }
        // set color based on fill amount
        Color32 color;
        if (0.5f < healthBar.fillAmount && healthBar.fillAmount <= 0.75f)
        {
            color = yellow;
        }
        else if (0.25f < healthBar.fillAmount && healthBar.fillAmount <= 0.5f)
        {
            color = orange;
        }
        else if (healthBar.fillAmount <= 0.25f)
        {
            color = red;
        }
        else
        {
            color = green;
        }
        if (color != healthBar.color)
        {
            healthBar.color = color;
        }
    }
}