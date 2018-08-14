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

        if(enemy.Movement == null || enemy.Movement.Navigation == null) { return; }

        if (enemy.Movement.CurrentMode != EnemyMovement.MovementMode.Running)
            EventManager.TriggerEvent<EnemyMovementEvent>(new EnemyMovementEvent(enemy, EnemyMovement.MovementMode.Running));

        if (!enemy.Movement.Navigation.destination.Equals(enemy.InitialPosition))
            enemy.Movement.MoveTo(enemy.InitialPosition);

        return;
    }
}
