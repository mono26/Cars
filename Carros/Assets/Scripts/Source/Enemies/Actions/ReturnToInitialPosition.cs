using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/ReturnToInitialPosition")]
public class ReturnToInitialPosition : AIAction
{
    public override void DoAction(Entity _entity)
    {
        GoInitialPosition(_entity);

        return;
    }

    protected void GoInitialPosition(Entity _entity)
    {
        if (_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            enemy.StartRunning();
            Vector3 initialPosition = enemy.GetInitialPosition;
            enemy.PatrolTowardsPoint(initialPosition);
        }
        return;
    }
}
