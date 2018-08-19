using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Actions/Enemy/PatrolAround")]
public class PatrolAround : AIAction
{
    public override void DoAction(Entity _entity)
    {
        Patrol(_entity);

        return;
    }

    protected void Patrol(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if(enemy == null) { return; }

        if (enemy.Targetter == null) { return; }
        if (enemy.Movement == null || enemy.Movement.Navigation == null) { return; }

        if (enemy.Movement.CurrentMode != EnemyMovement.MovementMode.Walking)
            EventManager.TriggerEvent<EnemyMovementEvent>(new EnemyMovementEvent(enemy, EnemyMovement.MovementMode.Walking));

        if (!enemy.Movement.Navigation.hasPath)
            enemy.Movement.NavigateTo(enemy.Targetter.CalculateRandomPointInsideTrigger());

        return;
    }
}
