using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/RunTowardsTarget")]
public class RunTowardsTarget : AIAction
{
    public override void DoAction(Entity _entity)
    {
        MakeItRunTowardsTarget(_entity);

        return;
    }

    protected void MakeItRunTowardsTarget(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return; }

        if (enemy.Targetter == null || enemy.Targetter.CurrentTarget == null || enemy.Targetter.CurrentSlot == null) { return; }

        if (enemy.Movement.CurrentMode != EnemyMovement.MovementMode.Running)
            EventManager.TriggerEvent<EnemyMovementEvent>(new EnemyMovementEvent(enemy, EnemyMovement.MovementMode.Running));

        enemy.Movement.MoveTo(enemy.Targetter.CurrentTarget.GetSlotPosition(enemy.Targetter.CurrentSlot));

        return;
    }
}
