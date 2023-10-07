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
    }

    private void OnDisable()
    {
        WeaponController.OnCurrentWeaponDataChanged -= UpadteWeaponUI;
    }

    private void UpadteWeaponUI(int currentBullets, int magSize)
    {
        ammoText.SetText($"{currentBullets} / {magSize}");
    }
}
