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
        if(_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            enemy.StartRunning();
            Vector3 positionToRunTo = enemy.GetTargetPosition();
            enemy.RunTowardsPoint(positionToRunTo);
        }
        return;
    }
}
