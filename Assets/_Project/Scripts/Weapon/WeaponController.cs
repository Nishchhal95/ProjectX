using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public static event Action<int, int> OnCurrentWeaponDataChanged;

    public WeaponData weaponData;

    public int WeaponID {  get; private set; }
    public string WeaponName { get; private set; }
    public int Damage { get; private set; }
    public float FireRate { get; private set; }
    public float AttackDistance { get; private set; }
    public int TotalAmmo { get; private set; }
    public int MagCapacity { get; private set; }
    public bool ReloadAble { get; private set; }
    public int CurrentBulletCountInMag { 
        get 
        { 
            return currentBulletCountInMag; 
        } 
        set 
        { 
            currentBulletCountInMag = value;
            OnCurrentWeaponDataChanged?.Invoke(currentBulletCountInMag, TotalAmmo);
        } 
    }
    private int currentBulletCountInMag;

    public void Init()
    {
        WeaponID = weaponData.weaponID;
        Damage = weaponData.damage;
        FireRate = weaponData.fireRate;
        TotalAmmo = weaponData.totalAmmo;
        MagCapacity = weaponData.magCapacity;
        WeaponName = weaponData.weaponName;
        AttackDistance = weaponData.attackDistance;
        ReloadAble = weaponData.reloadAble;

        CurrentBulletCountInMag = MagCapacity;
        TotalAmmo -= MagCapacity;
    }

    public virtual GameObject Attack (Transform shootPoint, LayerMask hittableMask)
    {
        CurrentBulletCountInMag--;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit, AttackDistance, hittableMask))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public virtual void Reload()
    {
        if (!ReloadAble)
        {
            return;
        }
        if(CurrentBulletCountInMag == MagCapacity) 
        {
            Debug.Log("No need to reload...");
            return;
        }

        if(TotalAmmo <= 0) 
        {
            Debug.Log("Cant reload, not enough bullets...");
            return;
        }

        int bulletsToTakeFromTotalAmmo = MagCapacity - CurrentBulletCountInMag;
        TotalAmmo -= bulletsToTakeFromTotalAmmo;
        CurrentBulletCountInMag += bulletsToTakeFromTotalAmmo;
    }
}
