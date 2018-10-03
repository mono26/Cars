// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar components")]
    [SerializeField] private Transform healthBar;
    [SerializeField] private Transform backGroundBar;

    private bool HasHealthBar()
    {
        bool hasHealthBar = true;
        try
        {
            if (healthBar == null)
            {
                hasHealthBar = false;
                throw new MissingComponentException(gameObject, typeof(Transform));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasHealthBar;
    }

    private bool HasHealthBackGroundBar()
    {
        bool hasBackGroundBar = true;
        try
        {
            if (backGroundBar == null)
            {
                hasBackGroundBar = false;
                throw new MissingComponentException(gameObject, typeof(Transform));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasBackGroundBar;
    }

    public void UpdateHealthBar(float _healthPercentage)
    {
        Vector3 barScale = Vector3.one;
        if (HasHealthBar())
        {
            barScale.x = _healthPercentage;
            healthBar.localScale = barScale;
        }
        if (HasHealthBackGroundBar())
        {
            barScale.x = 1 - _healthPercentage;
            backGroundBar.localScale = barScale;
        }
        return;
    }
}
