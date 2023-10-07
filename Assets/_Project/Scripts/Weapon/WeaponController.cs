using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public abstract class WeaponController
{
    public int weaponID;

    public GameObject modelPrefab;
    public int damage;
    public float fireRate;
    public int totalAmmo;
    public int magCapacity;
    public int currentBulletCountInMag;


    public static event Action<int, int> OnCurrentWeaponDataChanged;

    public WeaponController(WeaponData weaponData)
    {
        weaponID = weaponData.weaponID;
        modelPrefab = weaponData.modelPrefab;
        damage = weaponData.damage;
        fireRate = weaponData.fireRate;
        totalAmmo = weaponData.totalAmmo;
        magCapacity = weaponData.magCapacity;

        currentBulletCountInMag = magCapacity;
        totalAmmo -= magCapacity;

        OnCurrentWeaponDataChanged?.Invoke(currentBulletCountInMag, totalAmmo);
    }

    public void Equipped()
    {
        OnCurrentWeaponDataChanged?.Invoke(currentBulletCountInMag, totalAmmo);
    }

    public bool CanShoot()
    {
        return currentBulletCountInMag > 0;
    }

    public virtual GameObject Shoot(Transform shootPoint, LayerMask hittableMask)
    {
        currentBulletCountInMag--;
        OnCurrentWeaponDataChanged?.Invoke(currentBulletCountInMag, totalAmmo);
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit, 1000f, hittableMask))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public virtual void Reload()
    {
        if(currentBulletCountInMag == magCapacity) 
        {
            Debug.Log("No need to reload...");
            return;
        }

        if(totalAmmo <= 0) 
        {
            Debug.Log("Cant reload, not enough bullets...");
            return;
        }

        int bulletsToTakeFromTotalAmmo = magCapacity - currentBulletCountInMag;
        totalAmmo -= bulletsToTakeFromTotalAmmo;
        currentBulletCountInMag += bulletsToTakeFromTotalAmmo;

        OnCurrentWeaponDataChanged?.Invoke(currentBulletCountInMag, totalAmmo);
    }
}
