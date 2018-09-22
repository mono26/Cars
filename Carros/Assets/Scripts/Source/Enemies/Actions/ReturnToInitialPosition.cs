using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/ReturnToInitialPosition")]
public class ReturnToInitialPosition : AIAction
{
    public override void DoAction(Entity _entity)
    {
        GoToInitialPosition(_entity);
        return;
    }

    protected void GoToInitialPosition(Entity _entity)
    {
        if (_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            Vector3 initialPosition = enemy.GetInitialPosition;
            enemy.PatrolTowardsPoint(initialPosition);
        }
        return;
    }
}
