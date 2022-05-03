using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBar;

    void Awake()
    {   
        healthBar = gameObject.GetComponent<Image>();
    }

    public void DecreaseHealthBar(int currentHP, int maxHP)
    {
        float healthPercent = (float)currentHP/maxHP;
        if (healthPercent != healthBar.fillAmount)
        {
            healthBar.fillAmount = healthPercent;
        }
    }
}