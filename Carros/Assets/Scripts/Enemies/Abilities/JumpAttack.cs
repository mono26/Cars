using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : Ability
{
    [Header("Ram settings")]
    [SerializeField]
    protected float flightTime = 1.0f;
    [SerializeField]
    protected float jumpAngle = 45f;

    public override void Cast()
    {
        Jump();

        base.Cast();

        return;
    }

    protected void Jump()
    {
        Debug.Log(entity.gameObject.name + "Casting Ram");

        Targetter targetter = aiEntity.Targetter;
        if(targetter == null || targetter.CurrentTarget == null) { return; }

        Debug.Log(aiEntity.gameObject.name + " current body velocity " + aiEntity.Body.velocity);
        aiEntity.Body.velocity = Vector3.zero;
        //aiEntity.Navigation.isStopped = true;

        Vector3 initialPosition = aiEntity.transform.position;
        Vector3 targetPosition = targetter.CurrentTarget.transform.position;
        Vector3 jumpVelocity = CustomPhysics.CalculateJumpVelocityFromTimeAndAngle(initialPosition, targetPosition, jumpAngle, flightTime);

        entity.Body.velocity = jumpVelocity;

        return;
    }
}
