using UnityEngine;

public class Ram : Ability
{
    [Header("Ram settings")]
    [SerializeField]
    protected float ramForce = 10.0f;

    public override void Cast()
    {
        RamTowardsTarget();
        base.Cast();
        return;
    }

    protected void RamTowardsTarget()
    {
        Vector3 initialPosition = entity.transform.position;
        Vector3 targetPosition = GetTargetPosition();
        Vector3 targetDirection = (targetPosition - initialPosition);
        // The direction must be flat in XZ plane.
        targetDirection.y = 0;
        Vector3 normalizedDirection = targetDirection.normalized;
        entity.GetBody.AddForce(normalizedDirection * ramForce, ForceMode.Impulse);
        return;
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = entity.transform.position;
        if (entity is Enemy)
        {
            Enemy castingEnemy = entity as Enemy;
            targetPosition = castingEnemy.GetTargetPosition();
        }
        return targetPosition;
    }

    private void ReadyToRam()
    {
        entity.SetBodyVelocity(Vector3.zero);
        entity.BodyAffectedByGravity(true);
        if (entity is Enemy)
        {
            Enemy enemy = entity as Enemy;
            enemy.ActivateNavigation(false);
        }
        return;
    }
}
