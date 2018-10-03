// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Decision/CanRecastAbility")]
public class CanRecastAbility : AIDecision
{
    public override bool Decide(Entity _entity)
    {
        bool decision = false;
        decision = CanRecast(_entity);
        return decision;
    }

    protected bool CanRecast(Entity _entity)
    {
        bool canRecast = false;
        Enemy enemy = _entity as Enemy;
        if (_entity is Enemy)
        {
            if (enemy.GetNextAbility != null) {
                canRecast = true;
            }
        }
        return canRecast;
    }
}
