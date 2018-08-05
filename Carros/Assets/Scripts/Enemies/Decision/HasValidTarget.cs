using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Decision/HasValidTarget")]
public class HasValidTarget : AIDecision
{
    public override bool Decide(Entity _entity)
    {
        bool decision = false;
        decision = ValidTarget(_entity);

        return decision;
    }

    protected bool ValidTarget(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return false; }

        if(enemy.Targetter.CurrentTarget == null) { return false; }

        else { return true; }
    }
}
