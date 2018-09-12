using UnityEngine;

public class JumpAttack : Ability
{
    [Header("Jump Attack settings")]
    [SerializeField] private float jumpSpeed = 10.0f;

    public override void Cast()
    {
        JumpTowardsTarget();
        base.Cast();
        return;
    }

    private void JumpTowardsTarget()
    {
        // TODO refactorization,check conditions and early exit.
        Vector3 initialPosition = entity.transform.position;
        Vector3 targetPosition = GetTargetPosition();
        ReadyToJump();
        Vector3 jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, jumpSpeed);
        entity.SetBodyVelocity (jumpVelocity);
        return;
    }

    private void ReadyToJump()
    {
        entity.SetBodyVelocity (Vector3.zero);
        entity.BodyAffectedByGravity(true);
        if (entity is Enemy)
        {
            Enemy enemy = entity as Enemy;
            enemy.ActivateNavigation(false);
        }
        return;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = Vector3.zero;
        if (entity is Enemy)
        {
            Enemy castingEnemy = entity as Enemy;
            SlotTargetter slotTargetter = castingEnemy.Targetter as SlotTargetter;
            Targetter targetter = castingEnemy.Targetter;
            if (slotTargetter != null && slotTargetter.CurrentSlotTarget != null) { targetPosition = slotTargetter.CurrentSlotTarget.transform.position; }
            else if (targetter != null && targetter.CurrentTarget != null) { targetPosition = targetter.CurrentTarget.position; }
        }
        return targetPosition;
    }
}
