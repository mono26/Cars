using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/RunTowardsTarget")]
public class RunTowardsTarget : AIAction
{
    public override void DoAction(Entity _entity)
    {
        MakeItRun(_entity);

        return;
    }

    protected void MakeItRun(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return; }

        EnemyMovement movement = enemy.Movement;
        UnityEngine.AI.NavMeshAgent navigation = enemy.Movement.Navigation;
        if (movement == null || navigation == null) { return; }

        SlotTargetter slotTargetter = enemy.Targetter as SlotTargetter;
        if (slotTargetter != null && slotTargetter.CurrentSlotTarget != null && slotTargetter.CurrentSlot != null)
        {
            Vector3 slotPosition = slotTargetter.CurrentSlotTarget.GetSlotPosition(slotTargetter.CurrentSlot);
            SetNavigationPointAndMakeItRun(enemy, slotPosition);

            return;
        }

        Targetter targetter = enemy.Targetter;
        if (targetter != null && targetter.CurrentTarget != null)
        {
            Vector3 targetPosition = targetter.CurrentTarget.position;
            SetNavigationPointAndMakeItRun(enemy, targetPosition);

            return;
        }

        return;
    }

    protected void SetNavigationPointAndMakeItRun(Enemy _enemy, Vector3 _targetPoint)
    {
        EnemyMovement movement = _enemy.Movement;
        if (movement.CurrentMode != EnemyMovement.MovementMode.Running)
            EventManager.TriggerEvent<EnemyMovementEvent>(new EnemyMovementEvent(_enemy, EnemyMovement.MovementMode.Running));

        if (!movement.Navigation.destination.Equals(_targetPoint))
            movement.NavigateTo(_targetPoint);

        return;
    }
}
