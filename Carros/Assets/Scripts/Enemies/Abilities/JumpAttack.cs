using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : Ability
{
    [Header("Ram settings")]
    [SerializeField]
    protected float jumpHeight = 5.0f;
    [SerializeField]
    protected float jumpSpeed = 10.0f;

    public override void Cast()
    {
        Jump();

        base.Cast();

        return;
    }

    protected void Jump()
    {
        // TODO refactorization
        Debug.Log(entity.gameObject.name + "Casting Ram");

        Vector3 initialPosition = aiEntity.transform.position;
        Vector3 targetPosition = Vector3.zero;
        Vector3 jumpVelocity = Vector3.zero;

        SlotTargetter slotTargetter = aiEntity.Targetter as SlotTargetter;
        if(slotTargetter != null && slotTargetter.CurrentSlotTarget != null)
        {
            aiEntity.Body.velocity = Vector3.zero;
            aiEntity.Movement.ActivateNavigation(false);

            targetPosition = slotTargetter.CurrentSlotTarget.transform.position;
            jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, jumpSpeed);
            entity.Body.velocity = jumpVelocity;
            return;
        }

        Targetter targetter = aiEntity.Targetter;
        if (targetter != null && targetter.CurrentTarget != null)
        {
            aiEntity.Body.velocity = Vector3.zero;
            aiEntity.Movement.ActivateNavigation(false);

            targetPosition = targetter.CurrentTarget.position;
            jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, jumpSpeed);
            entity.Body.velocity = jumpVelocity;
            return;
        }

        return;
    }
}
