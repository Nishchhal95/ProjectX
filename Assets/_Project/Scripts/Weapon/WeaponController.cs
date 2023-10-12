using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public static event Action<int, int> OnCurrentWeaponDataChanged;

    public WeaponData weaponData;

    [field: SerializeField] public int WeaponID {  get; private set; }
    [field: SerializeField]public string WeaponName { get; private set; }
    [field: SerializeField]public int Damage { get; private set; }
    [field: SerializeField]public float FireRate { get; private set; }
    [field: SerializeField]public float AttackDistance { get; private set; }
    [field: SerializeField]public int TotalAmmo { get; private set; }
    [field: SerializeField]public int MagCapacity { get; private set; }
    [field: SerializeField]public bool ReloadAble { get; private set; }

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

    //TODO : Introduce a GunWeaponController for guns which would segregate it from being a knife or something else.
    public bool CanShoot()
    {
        return currentBulletCountInMag > 0;
    }

    public virtual HitInfo Attack (Transform shootPoint, LayerMask hittableMask)
    {
        //TODO : Introduce a GunWeaponController for guns which would segregate it from being a knife or something else.
        if (!CanShoot() && ReloadAble)
        {
            return null;
        }
        CurrentBulletCountInMag--;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit, AttackDistance, hittableMask))
        {
            return new HitInfo()
            {
                HitObject = hit.collider.gameObject,
                HitPoint = hit.point
            };
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

public class HitInfo
{
    public GameObject HitObject { get; set; }
    public Vector3 HitPoint { get; set; }
}
