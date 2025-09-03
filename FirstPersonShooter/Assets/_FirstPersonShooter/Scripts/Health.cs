using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    
    [Header("UI")]
    public TextMeshProUGUI healthText;

    public void TakeDamage(int damage)
    {
        health -= damage;

        healthText.text = health.ToString();
        
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
