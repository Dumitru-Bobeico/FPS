using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health;
    
    public RectTransform healthBar;
    private float originalHealthBarSize;
    
    [Header("UI")]
    public TextMeshProUGUI healthText;

    private void Start()
    {
        originalHealthBarSize = healthBar.sizeDelta.x;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        healthBar.sizeDelta = new Vector2(originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);

        healthText.text = health.ToString();
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
