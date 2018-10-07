// Copyright (c) What a Box Creative Studio. All rights reserved.

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
        ReadyToJump();
        Vector3 initialPosition = entity.transform.position;
        Vector3 targetPosition = RetrieveTargetPosition();
        Vector3 jumpVelocity = CalculateJumpVelocity(initialPosition, targetPosition);
        entity.SetBodyVelocity(jumpVelocity);
        return;
    }

    private void ReadyToJump()
    {
        entity.SetBodyVelocity(Vector3.zero);
        entity.SetIfBodyAffectedByGravity(true);
        if (entity is Enemy)
        {
            Enemy enemy = entity as Enemy;
            enemy.ActivateNavigation(false);
        }
        return;
    }

    private Vector3 CalculateJumpVelocity(Vector3 initialPosition, Vector3 targetPosition)
    {
        float relativeY = initialPosition.y - targetPosition.y;
        BallisticArcPreference arcPreference;
        if (Mathf.Abs(relativeY) <= 0.5) {
            arcPreference = BallisticArcPreference.ParabolicArc;
        }
        else {
            arcPreference = BallisticArcPreference.DirectArc;
        }
        Vector3 jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(
            new ParabolicMovementData(initialPosition, targetPosition, jumpSpeed, arcPreference)
            );
        return jumpVelocity;
    }
}
