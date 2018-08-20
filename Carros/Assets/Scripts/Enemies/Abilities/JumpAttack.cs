using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : Ability
{
    [Header("Jump Attack settings")]
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
        Debug.Log(entity.gameObject.name + "Casting Jump Attack");

        Vector3 initialPosition = aiEntity.transform.position;
        Vector3 targetPosition = Vector3.zero;
        Vector3 jumpVelocity = Vector3.zero;

        SlotTargetter slotTargetter = aiEntity.Targetter as SlotTargetter;
        Targetter targetter = aiEntity.Targetter;
        if (slotTargetter != null && slotTargetter.CurrentSlotTarget != null) { targetPosition = slotTargetter.CurrentSlotTarget.transform.position; }
        else if (targetter != null && targetter.CurrentTarget != null) { targetPosition = targetter.CurrentTarget.position; }

        aiEntity.Body.velocity = Vector3.zero;
        aiEntity.Movement.ActivateNavigation(false);

        jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, jumpSpeed);
        entity.Body.velocity = jumpVelocity;

        return;
    }
}
