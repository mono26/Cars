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
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return; }

        if(enemy.Movement == null || enemy.Movement.GetNavigationComponent == null) { return; }

        if (enemy.Movement.GetCurrentMode != EnemyMovement.MovementMode.Running)
            EventManager.TriggerEvent<EnemyMovementEvent>(new EnemyMovementEvent(enemy, EnemyMovement.MovementMode.Running));

        if (!enemy.Movement.GetNavigationComponent.destination.Equals(enemy.InitialPosition))
            enemy.Movement.SetNavigationDestination(enemy.InitialPosition);

        return;
    }
}
