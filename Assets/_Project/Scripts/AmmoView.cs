using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private void OnEnable()
    {
        WeaponController.OnCurrentWeaponDataChanged += UpadteWeaponUI;
        PlayerWeaponController.OnNewWeaponEquipped += UpadteWeaponUI;
    }

    private void OnDisable()
    {
        WeaponController.OnCurrentWeaponDataChanged -= UpadteWeaponUI;
        PlayerWeaponController.OnNewWeaponEquipped -= UpadteWeaponUI;
    }

    private void UpadteWeaponUI(int currentBullets, int magSize)
    {
        if(currentBullets < 0 || magSize < 0) 
        {
            ammoText.gameObject.SetActive(false);
            return;
        }

        if (!ammoText.gameObject.activeSelf)
        {
            ammoText.gameObject.SetActive(true);
        }
        ammoText.SetText($"{currentBullets} / {magSize}");
    }
}
