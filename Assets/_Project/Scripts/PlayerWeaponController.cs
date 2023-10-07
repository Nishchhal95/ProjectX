using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerWeaponController : NetworkBehaviour
{
    [SerializeField] private Transform weaponHolderTransform;
    [SerializeField] private Transform cameraTranform;
    [SerializeField] private LayerMask hittableMask;

    [SerializeField] private WeaponController equippedWeapon;

    private InputManager inputManager;

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            cameraTranform.gameObject.SetActive(true);
            equippedWeapon = new WeaponController(new Weapon
            {
                Damage = 30,
                FireRate = 1,
                MagCapacity = 30,
                TotalAmmo = 60
            });
        }
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        HandleShooting();
        HandleReloading();
    }

    private void HandleReloading()
    {
        if (!inputManager.IsReloadPressed() || equippedWeapon == null)
        {
            return;
        }

        equippedWeapon.Reload();
    }

    private void HandleShooting()
    {
        if(!inputManager.IsShootPressed() || equippedWeapon == null || !equippedWeapon.CanShoot())
        {
            return;
        }

        equippedWeapon.Shoot();
        Shoot();
    }

    private void Shoot()
    {
        if (Physics.Raycast(cameraTranform.position,
            cameraTranform.forward, out RaycastHit hit, 1000f, hittableMask))
        {
            // If I hit a player then damage them
            if(hit.collider.gameObject.TryGetComponent(out PlayerStatsController playerStatsController))
            {
                DamageOpponentServerRpc(30, NetworkObjectId, playerStatsController.NetworkObjectId);
            }
            else
            {
                //Spawn a bullet hole

            }
        }
    }

    [ServerRpc]
    private void DamageOpponentServerRpc(int damageAmount, ulong sourceNetworkObjectId, ulong targetNetworkObjectId)
    {
        Debug.Log($"SERVER--: {sourceNetworkObjectId} did {damageAmount} damage to {targetNetworkObjectId}");
        DamageTakenClientRpc(damageAmount, sourceNetworkObjectId, targetNetworkObjectId);
    }

    [ClientRpc]
    private void DamageTakenClientRpc(int damageAmount, ulong sourceNetworkObjectId, ulong targetNetworkObjectId)
    {
        Debug.Log($"CLIENT--: {sourceNetworkObjectId} did {damageAmount} damage to {targetNetworkObjectId}");
        PlayerStatsController.playerStatsSystems[targetNetworkObjectId].TakeDamage(damageAmount);
    }
}
