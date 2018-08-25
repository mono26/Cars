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

        float stopDistance = enemy.Movement.Navigation.stoppingDistance;
        Debug.Log((Vector3.Distance(enemy.transform.position, enemy.InitialPosition) - stopDistance).ToString());
        if (Vector3.Distance(enemy.transform.position, enemy.InitialPosition) - stopDistance < stopDistance)
            return true;

        return false;
    }
}
