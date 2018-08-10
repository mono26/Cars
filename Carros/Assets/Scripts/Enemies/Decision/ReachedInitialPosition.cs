using UnityEngine;

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

        if (Vector3.Distance(enemy.transform.position, enemy.InitialPosition) < enemy.Movement.Navigation.stoppingDistance)
            return true;

        return false;
    }
}
