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

        if (!enemy.Targetter.CurrentTarget) { return; }

        enemy.Movement.SetDestination(enemy.Targetter.CurrentTarget.GetSlotPosition(enemy.Targetter.CurrentTargetSlot));

        /*Vector3 directionToTarget = Vector3.zero;
        directionToTarget = enemy.Target.position - enemy.transform.position;
        enemy.Movement.SetMoveDirection(directionToTarget.normalized);
        //enemy.Movement.IsFacingMovementDirection = true;*/

        return;
    }
}
