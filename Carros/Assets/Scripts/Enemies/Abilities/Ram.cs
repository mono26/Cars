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

        if(aiEntity.Targetter.CurrentTarget == null) { return; }

        aiEntity.Body.velocity = Vector3.zero;
        //aiEntity.Navigation.isStopped = true;

        entity.Body.AddForce(transform.up * ramForce , ForceMode.Impulse);
        Vector3 directionToTarget = aiEntity.Targetter.CurrentTarget.transform.position - aiEntity.transform.position;
        entity.Body.AddForce(directionToTarget.normalized * ramForce, ForceMode.Impulse);

        return;
    }
}
