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
        Debug.Log(entity.gameObject.name + "Casting Ram");

        entity.Body.velocity = Vector3.zero;
        entity.Body.AddForce(transform.up * ramForce * 0.9f, ForceMode.Impulse);
        entity.Body.AddForce(transform.right * ramForce, ForceMode.Impulse);

        return;
    }
}
