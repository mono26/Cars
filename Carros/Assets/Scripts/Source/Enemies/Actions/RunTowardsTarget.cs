// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/RunTowardsTarget")]
public class RunTowardsTarget : AIAction
{
    public override void DoAction(Entity _entity)
    {
        MakeItRunTowards(_entity);
        return;
    }

    protected void MakeItRunTowards(Entity _entity)
    {
        if(_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            Vector3 positionToRunTo = enemy.GetTargetPosition();
            enemy.RunTowardsPoint(positionToRunTo);
        }
        return;
    }
}
