// Copyright (c) What a Box Creative Studio. All rights reserved.

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
        ReadyToRam();
        Vector3 initialPosition = entity.transform.position;
        Vector3 targetPosition = RetrieveTargetPosition();
        Vector3 targetDirection = (targetPosition - initialPosition);
        // The direction must be flat in XZ plane.
        targetDirection.y = 0;
        Vector3 normalizedDirection = targetDirection.normalized;
        entity.GetBody.AddForce(normalizedDirection * ramForce, ForceMode.Impulse);
        return;
    }

    private void ReadyToRam()
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
}
