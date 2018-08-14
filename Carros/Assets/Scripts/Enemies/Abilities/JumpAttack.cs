using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : Ability
{
    [Header("Ram settings")]
    [SerializeField]
    protected float jumpHeight = 5.0f;

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

        //aiEntity.Body.velocity = Vector3.zero;
        //aiEntity.Movement.Navigation.isStopped = true;

        Vector3 initialPosition = aiEntity.transform.position;
        Vector3 targetPosition = targetter.CurrentTarget.transform.position;
        Vector3 jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, jumpHeight);
        Debug.Log("Jump velocity" + jumpVelocity.ToString());

        entity.Body.velocity = jumpVelocity;

        return; 
    }
}
