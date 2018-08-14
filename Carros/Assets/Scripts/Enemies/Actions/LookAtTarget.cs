using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/LookAtTarget")]
public class LookAtTarget : AIAction
{
    public override void DoAction(Entity _entity)
    {
        LookAt(_entity);

        return;
    }

    protected void LookAt(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return; }

        Targetter targetter = enemy.Targetter;
        if (targetter == null || targetter.CurrentTarget == null) { return; }

        Vector3 directionToTarget = targetter.CurrentTarget.transform.position - enemy.transform.position;
        directionToTarget.y = 0;
        enemy.transform.rotation = Quaternion.LookRotation(directionToTarget.normalized);

        return;
    }
}
