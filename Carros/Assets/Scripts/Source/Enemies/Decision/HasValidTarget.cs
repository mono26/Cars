using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Decision/HasValidTarget")]
public class HasValidTarget : AIDecision
{
    public override bool Decide(Entity _entity)
    {
        bool decision = false;
        decision = CheckForValidTarget(_entity);

        return decision;
    }

    protected bool CheckForValidTarget(Entity _entity)
    {
        bool hasTarget = true;
        if(_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            hasTarget = enemy.HasValidTarget();
        }
        return hasTarget;
    }
}
