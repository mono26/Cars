﻿using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Decision/ReachedInitialPosition")]
public class ReachedInitialPosition : AIDecision
{
    public override bool Decide(Entity _entity)
    {
        bool decision = false;
        decision = InIinitialPosition(_entity);

        return decision;
    }

    protected bool InIinitialPosition(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return false; }

        float stopDistance = enemy.GetMovementComponent.GetNavigationComponent.stoppingDistance;
        if (Vector3.Distance(enemy.transform.position, enemy.GetInitialPosition) - stopDistance < stopDistance)
            return true;

        return false;
    }
}
