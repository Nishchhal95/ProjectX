using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerWeaponController : NetworkBehaviour
{
    public static event Action<int, int> OnNewWeaponEquipped;

    [SerializeField] private Transform weaponHolderTransform;
    [SerializeField] private Transform cameraTranform;
    [SerializeField] private LayerMask hittableMask;

    [SerializeField] private WeaponController equippedWeapon;

    [SerializeField] private WeaponController primaryWeapon;
    [SerializeField] private WeaponController secondaryWeapon;
    [SerializeField] private WeaponController meleeWeapon;

    //TODO: Maybe also look into making this dynamic.
    [SerializeField] private WeaponController[] weaponControllers;
    [SerializeField] private AudioSource audioSource;

    private InputManager inputManager;

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    public override void OnNetworkSpawn()
    {
        SetupWeapons();
    }

    private void SetupWeapons()
    {
        primaryWeapon = Instantiate(weaponControllers[0], weaponHolderTransform);
        secondaryWeapon = Instantiate(weaponControllers[1], weaponHolderTransform);
        meleeWeapon = Instantiate(weaponControllers[2], weaponHolderTransform);

        primaryWeapon.Init();
        secondaryWeapon.Init();
        meleeWeapon.Init();

        secondaryWeapon.gameObject.SetActive(false);
        meleeWeapon.gameObject.SetActive(false);

        EquipWeapon(primaryWeapon);
    }

    private void Update()
    {
        HandleWeaponSwitching();
        HandleWeaponAttack();
        HandleReload();
    }

    private void HandleWeaponSwitching()
    {
        if (inputManager.IsPrimaryWeaponKeyPressed())
        {
            EquipWeapon(primaryWeapon);
        }

        if (inputManager.IsSecondaryWeaponKeyPressed())
        {
            EquipWeapon(secondaryWeapon);
        }

        if (inputManager.IsMeleeWeaponKeyPressed())
        {
            EquipWeapon(meleeWeapon);
        }
    }

    private void HandleWeaponAttack()
    {
        if(equippedWeapon == null)
        {
            return;
        }

        if (inputManager.IsShootPressed() && equippedWeapon.CanShoot())
        {
            //TODO : Maybe use an interface here, in case in future if we want to make other things damageable too.
            HandleDealingDamage(equippedWeapon.Attack(cameraTranform, hittableMask));
            audioSource.clip = equippedWeapon.weaponData.shotSound;
            audioSource.Play();
        }
    }

    private void HandleReload()
    {
        if (inputManager.IsReloadPressed())
        {
            equippedWeapon.Reload();
            audioSource.clip = equippedWeapon.weaponData.reloadSound;
            audioSource.Play();
        }
    }

    private void EquipWeapon(WeaponController weapon)
    {
        equippedWeapon?.gameObject.SetActive(false);

        equippedWeapon = weapon;
        equippedWeapon.gameObject.SetActive(true);
        OnNewWeaponEquipped?.Invoke(equippedWeapon.CurrentBulletCountInMag, equippedWeapon.TotalAmmo);
    }

    private void HandleDealingDamage(HitInfo hitInfo)
    {
        if (hitInfo == null)
        {
            return;
        }
        if (hitInfo.HitObject.TryGetComponent(out PlayerStatsController playerStatsController))
        {
            //TODO : Fix Self damage lol
            DamageOpponentServerRpc(equippedWeapon.Damage, NetworkObjectId, playerStatsController.NetworkObjectId);
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
