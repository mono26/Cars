using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/RunTowardsTarget")]
public class RunTowardsTarget : AIAction
{
    public override void DoAction(Entity _entity)
    {
        MakeEnemyRun(_entity);

        return;
    }

    protected void MakeEnemyRun(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        Vector3 directionToTarget = Vector3.zero;
        if (enemy != null)
        {
            directionToTarget = enemy.Target.position - enemy.transform.position;
            enemy.Movement.SetMoveDirection(directionToTarget.normalized);
        }


        return;
    }
}
