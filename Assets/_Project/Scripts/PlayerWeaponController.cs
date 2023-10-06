using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponHolderTransform;
    [SerializeField] private Transform cameraTranform;
    [SerializeField] private LayerMask hittableMask;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if(inputManager.IsShootPressed())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(cameraTranform.position,
            cameraTranform.forward, out RaycastHit hit, 1000f, hittableMask))
        {
            Debug.Log($"I Hit {hit.collider.name}");

            // If I hit a player then damage them
            if(hit.collider.gameObject.TryGetComponent(out PlayerStatsController playerStatsController))
            {
                playerStatsController.TakeDamage(30);
            }
        }
    }
}
