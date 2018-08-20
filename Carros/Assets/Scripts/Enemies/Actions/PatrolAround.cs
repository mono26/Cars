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
        // TODO refactorization
        Enemy enemy = _entity as Enemy;
        if(enemy == null) { return; }

        Targetter targetter = enemy.Targetter;
        if (targetter == null) { return; }

        EnemyMovement movement = enemy.Movement;
        UnityEngine.AI.NavMeshAgent navigation = enemy.Movement.Navigation;
        if (movement == null || navigation == null) { return; }

        if (movement.CurrentMode != EnemyMovement.MovementMode.Walking)
            EventManager.TriggerEvent<EnemyMovementEvent>(new EnemyMovementEvent(enemy, EnemyMovement.MovementMode.Walking));

        if (!navigation.hasPath)
            movement.NavigateTo(targetter.CalculateRandomPointInsideTrigger());

        return;
    }
}
