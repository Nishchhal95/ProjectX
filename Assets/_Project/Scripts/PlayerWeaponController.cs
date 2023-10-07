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

    [SerializeField] private WeaponController primaryWeapon;
    [SerializeField] private WeaponController secondaryWeapon;
    [SerializeField] private WeaponController meleeWeapon;

    [SerializeField] private WeaponData[] weaponDatas;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = InputManager.Instance;
        primaryWeapon = new VandalWeaponController(weaponDatas[0]);
        EquipWeapon(primaryWeapon);
    }

    private void Update()
    {
        HandleWeaponSwitching();
        HandleShooting();
        HandleReloading();
    }

    private void HandleWeaponSwitching()
    {
        if(inputManager.IsPrimaryWeaponKeyPressed())
        {
            if(primaryWeapon == null)
            {
                primaryWeapon = new VandalWeaponController(weaponDatas[0]);
            }
            EquipWeapon(primaryWeapon);
        }

        if (inputManager.IsSecondaryWeaponKeyPressed())
        {
            if (secondaryWeapon == null)
            {
                secondaryWeapon = new GlockWeaponController(weaponDatas[1]);
            }
            EquipWeapon(secondaryWeapon);
        }

        if (inputManager.IsMeleeWeaponKeyPressed())
        {
            if (meleeWeapon == null)
            {
                meleeWeapon = new MeleeWeaponController(weaponDatas[2]);
            }
            EquipWeapon(meleeWeapon);
        }
    }

    private void EquipWeapon(WeaponController weaponController)
    {
        if(equippedWeapon?.weaponID == weaponController?.weaponID)
        {
            return;
        }

        if (weaponHolderTransform.childCount > 0)
        {
            Destroy(weaponHolderTransform.GetChild(0).gameObject);
        }

        equippedWeapon = weaponController;
        equippedWeapon.Equipped();
        GameObject weaponModel = Instantiate(equippedWeapon.modelPrefab, weaponHolderTransform);
        weaponModel.transform.localPosition = Vector3.zero;
        weaponModel.transform.localRotation = Quaternion.identity;
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

        ProcessShot(equippedWeapon.Shoot(cameraTranform, hittableMask));
    }
 
    private void ProcessShot(GameObject shotObject)
    {
        if(shotObject == null)
        {
            return;
        }
        if (shotObject.TryGetComponent(out PlayerStatsController playerStatsController))
        {
            DamageOpponentServerRpc(equippedWeapon.damage, NetworkObjectId, playerStatsController.NetworkObjectId);
        }
        else
        {
            //Spawn a bullet hole

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
        PlayerNetworkController.GetPlayerNetworkController(targetNetworkObjectId).PlayerStatsController.TakeDamage(damageAmount);
    }
}
