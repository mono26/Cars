// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/ExecuteAbility")]
public class ExecuteAbility : AIAction
{
    public override void DoAction(Entity _entity)
    {
        CastAbility(_entity);

        return;
    }

    protected void CastAbility(Entity _entity)
    {
        if(_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            if (enemy.GetNextAbility)
            {
                enemy.GetNextAbility.Cast();
                enemy.SetNextAbility(null);
            }
        }
        return;
    }
}
