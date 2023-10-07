using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController
{
    private int currentBulletsLeft = 0;
    private int totalAmmo = 0;
    private int magCapacity = 0;
    private int damage = 0;
    private int fireRate = 0;

    public static Action<int, int> OnCurrentWeaponDataChanged;
    
    public WeaponController(Weapon weapon)
    {
        totalAmmo = weapon.TotalAmmo;
        magCapacity = weapon.MagCapacity;
        damage = weapon.Damage;
        fireRate = weapon.FireRate;
        currentBulletsLeft = magCapacity;
        totalAmmo = totalAmmo - magCapacity;

        OnCurrentWeaponDataChanged?.Invoke(currentBulletsLeft, totalAmmo);
    }

    public bool CanShoot()
    {
        return currentBulletsLeft > 0;
    }

    public void Shoot()
    {
        currentBulletsLeft -= 1;
        OnCurrentWeaponDataChanged?.Invoke(currentBulletsLeft, totalAmmo);
    }

    public void Reload()
    {
        if(currentBulletsLeft >= magCapacity)
        {
            Debug.Log("No reload required..");
            return;
        }

        int missingBulletCount = magCapacity - currentBulletsLeft;

        if (totalAmmo - missingBulletCount < 0)
        {
            Debug.Log("Not enough ammo");
            return;
        }

        if (missingBulletCount > 0)
        {
            currentBulletsLeft += missingBulletCount;
            totalAmmo -= missingBulletCount;
            totalAmmo = Mathf.Max(0, totalAmmo);
        }

        OnCurrentWeaponDataChanged?.Invoke(currentBulletsLeft, totalAmmo);
    }
}

public class Weapon
{
    public int TotalAmmo { get; set; }
    public int MagCapacity { get; set; }
    public int Damage { get; set; }
    public int FireRate { get; set; }
}
