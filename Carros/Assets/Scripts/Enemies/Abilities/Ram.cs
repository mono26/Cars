using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Ability
{
    [Header("Ram settings")]
    [SerializeField]
    protected float ramForce = 10.0f;

    public override void Cast()
    {
        RamTowardsTarget();

        base.Cast();

        return;
    }

    protected void RamTowardsTarget()
    {
        // TODO refactorization

        Vector3 initialPosition = entity.transform.position;
        Vector3 targetPosition = Vector3.zero;
        Vector3 targetDirection = Vector3.zero;

        Enemy castinEntity = entity as Enemy;
        SlotTargetter slotTargetter = castinEntity.Targetter as SlotTargetter;
        Targetter targetter = castinEntity.Targetter;
        if (slotTargetter != null && slotTargetter.CurrentSlotTarget != null) { targetPosition = slotTargetter.CurrentSlotTarget.transform.position; }
        else if (targetter != null && targetter.CurrentTarget != null) { targetPosition = targetter.CurrentTarget.position;}

        entity.Body.velocity = Vector3.zero;
        castinEntity.Movement.NavigationSetActive(false);

        targetDirection = (targetPosition - initialPosition);
        targetDirection.y = 0;
        targetDirection = targetDirection.normalized;
        entity.Body.AddForce(targetDirection * ramForce, ForceMode.Impulse);

        return;
    }
}
