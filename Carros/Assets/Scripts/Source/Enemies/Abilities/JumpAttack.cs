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
        Vector3 initialPosition = entity.transform.position;
        Vector3 targetPosition = GetTargetPosition();
        ReadyToJump();
        Vector3 jumpVelocity = CustomPhysics.CalculateVelocityVectorForParabolicMovement(
            initialPosition, 
            targetPosition, 
            jumpSpeed
            );
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

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = entity.transform.position;
        if (entity is Enemy)
        {
            Enemy castingEnemy = entity as Enemy;
            targetPosition = castingEnemy.GetTargetPosition();
        }
        return targetPosition;
    }
}
