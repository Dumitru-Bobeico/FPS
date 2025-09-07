using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int health;

    [Header("UI")]
    public RectTransform healthBar;
    public TextMeshProUGUI healthText;

    [Header("Respawn")]
    public Transform respawnPoint; // assign in inspector
    public float respawnDelay = 1f;

    private float originalHealthBarSize;

    private void Start()
    {
        health = maxHealth;
        if (healthBar != null)
            originalHealthBarSize = healthBar.sizeDelta.x;

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();

        if (health <= 0)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();
    }

    public void SetHealth(int value)
    {
        health = Mathf.Clamp(value, 0, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.sizeDelta = new Vector2(
                originalHealthBarSize * health / (float)maxHealth,
                healthBar.sizeDelta.y
            );
        }

        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }

    private void Respawn()
    {
        health = maxHealth;
        UpdateHealthUI();

        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        // optional: reset velocity if using Rigidbody or CharacterController
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        var controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            // small hack to reset controller's internal velocity
            controller.enabled = false;
            controller.enabled = true;
        }

        Debug.Log($"{gameObject.name} has respawned!");
    }
}
