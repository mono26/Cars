using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Ability
{
    [Header("Ram settings")]
    [SerializeField]
    protected float ramForce = 100f;

    public override void Cast()
    {
        PerformRam();

        return;
    }

    protected void PerformRam()
    {
        entity.Body.AddForce(transform.right * ramForce, ForceMode.Impulse);

        return;
    }
}
