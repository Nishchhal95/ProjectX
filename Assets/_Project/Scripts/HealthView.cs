using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthSlider;

    public void UpdateUI(int currentHealth, int maxHealth)
    {
        healthText.SetText($"{currentHealth} / {maxHealth}");
        healthSlider.fillAmount = GameHelper.Remap(currentHealth, 0, maxHealth, 0, 1);
    }
}
