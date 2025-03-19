using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;   // Reference to the health slider
    public EnemyFollow enemy;     // Reference to the EnemyFollow script

    void Update()
    {
        // Update the health slider value based on the enemy's health
        if (enemy != null)
        {
            healthSlider.value = enemy.currentHealth / enemy.maxHealth;
        }
    }
}
