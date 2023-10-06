using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    private int MAX_HEALTH = 100;
    private int currentHealth = 0;

    [SerializeField] private HealthView healthView;

    private void Start()
    {
        SetHealth(MAX_HEALTH);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            Heal(10);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MAX_HEALTH);

        if(currentHealth == 0)
        {
            //DEAD

        }
        UpdateUI();
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, MAX_HEALTH);
        UpdateUI();
    }

    private void UpdateUI()
    {
        healthView.UpdateUI(currentHealth, MAX_HEALTH);
    }

    private void SetHealth(int health)
    {
        currentHealth = health;
    }
}
