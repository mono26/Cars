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
        if(_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            Vector3 positionToPatrolTo = enemy.CalculateRandomPatrolPoint();
            enemy.PatrolTowardsPoint(positionToPatrolTo);
        }
        return;
    }
}
