using UnityEngine;

public class JumpAttack : Ability
{
    [Header("Jump Attack settings")]
    [SerializeField]
    protected float jumpSpeed = 10.0f;

    public override void Cast()
    {
        JumpTowardsTarget();

        base.Cast();

        return;
    }

    protected void JumpTowardsTarget()
    {
        // TODO refactorization,check conditions and early exit.

        Vector3 initialPosition = entity.transform.position;
        Vector3 targetPosition = Vector3.zero;
        Vector3 jumpVelocity = Vector3.zero;

        Enemy castingEnemy = entity as Enemy;
        SlotTargetter slotTargetter = castingEnemy.Targetter as SlotTargetter;
        Targetter targetter = castingEnemy.Targetter;
        if (slotTargetter != null && slotTargetter.CurrentSlotTarget != null) { targetPosition = slotTargetter.CurrentSlotTarget.transform.position; }
        else if (targetter != null && targetter.CurrentTarget != null) { targetPosition = targetter.CurrentTarget.position; }

        entity.GetBody.velocity = Vector3.zero;
        castingEnemy.Movement.NavigationSetActive(false);

        jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(initialPosition, targetPosition, jumpSpeed);
        entity.GetBody.velocity = jumpVelocity;

        return;
    }
}
