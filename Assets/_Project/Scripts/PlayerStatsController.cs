using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatsController : NetworkBehaviour
{
    private int MAX_HEALTH = 100;
    private int currentHealth = 0;

    [SerializeField] private HealthView healthView;

    public static Action<int, int> OnLocalPlayerHealthChanged;


    private void Start()
    {
        SetHealth(MAX_HEALTH);
    }

    private void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
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
        if(!IsLocalPlayer)
        {
            return;
        }
        OnLocalPlayerHealthChanged?.Invoke(currentHealth, MAX_HEALTH);
    }

    private void SetHealth(int health)
    {
        currentHealth = health;
    }
}
