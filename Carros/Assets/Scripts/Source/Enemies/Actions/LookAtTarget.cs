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
        if (_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            Vector3 directionToTarget = enemy.GetTargetPosition() - enemy.transform.position;
            directionToTarget.y = 0;
            enemy.transform.rotation = Quaternion.LookRotation(directionToTarget.normalized);
        }
        return;
    }
}
