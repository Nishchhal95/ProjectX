using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponData", order = 2, fileName = "NewWeaponData")]
public class WeaponData : ScriptableObject
{
    public int weaponID;
    public GameObject modelPrefab;
    public int damage;
    public float fireRate;
    public int totalAmmo;
    public int magCapacity;
}
