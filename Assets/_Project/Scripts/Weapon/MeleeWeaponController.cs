using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : WeaponController
{
    public MeleeWeaponController(WeaponData weaponData) : base(weaponData)
    {
        
    }

    public override bool CanShoot()
    {
        return true;
    }

    public override void Reload()
    {
        
    }
}
