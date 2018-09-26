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
        bool reachedInitialPosition = false;
        if(_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            float stopDistance = enemy.GetMovementComponent.GetNavigationComponent.stoppingDistance;
            if (Vector3.Distance(enemy.transform.position, enemy.GetInitialPosition) - stopDistance < stopDistance) {
                reachedInitialPosition = true;
            }
        }
        return reachedInitialPosition;
    }
}
