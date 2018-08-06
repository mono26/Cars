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

        Debug.Log(enemy.Targetter.CurrentTarget.GetSlotPosition(enemy.Targetter.CurrentSlot).ToString());
        enemy.Movement.SetDestination(enemy.Targetter.CurrentTarget.GetSlotPosition(enemy.Targetter.CurrentSlot));

        return;
    }
}
