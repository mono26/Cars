// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Health Component settings")]
    [SerializeField] private int maxHealth;

    [Header("Health Component components")]
    [SerializeField] private HealthBar healthDisplay; 

    [Header("Health Component editor debugging")]
    [SerializeField] private int currentHealth;

	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        return;
	}

    private void UpdateHealthBar()
    {
        if(healthDisplay != null) {
            healthDisplay.UpdateHealthBar(currentHealth / maxHealth);
        }
        return;
    }

    public void ReceiveDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && healthDisplay != null) {
            // TODO proper kill or release.
            Destroy(this.gameObject);
        }
        UpdateHealthBar();
        return;
    }

}
