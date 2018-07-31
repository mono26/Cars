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
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return false; }

        if (enemy.NextAbility != null)
            return true;

        return false;
    }
}
