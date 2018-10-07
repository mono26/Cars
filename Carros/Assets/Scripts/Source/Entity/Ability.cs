// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public abstract class Ability : EntityComponent
{
    [Header("Ability settings")]
    [SerializeField] private float cooldown;
    [SerializeField] private float range;

    private float lastCast = 0;

    public float GetRange { get { return range; } }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(entity.transform.position, range);
        return;
    }

    protected virtual void Start()
    {
        lastCast = Time.timeSinceLevelLoad - cooldown;
        return;
    }

    protected Vector3 RetrieveTargetPosition()
    {
        Vector3 targetPosition = entity.transform.position;
        if (entity is Enemy)
        {
            Enemy castingEnemy = entity as Enemy;
            targetPosition = castingEnemy.GetTargetPosition();
        }
        return targetPosition;
    }

    public virtual void Cast()
    {
        lastCast = Time.timeSinceLevelLoad;
        return;
    }

    public bool IsInCooldown()
    {
        bool isInCooldown = true;
        if (Time.timeSinceLevelLoad > lastCast + cooldown) {
            isInCooldown = false;
        }
        return isInCooldown;
    }

    public bool IsInRange(Vector3 _targetPosition)
    {
        bool isInRange = true;
        float distance = Vector3.Distance(entity.transform.position, _targetPosition);
        if (distance > range) {
            isInRange = false;
        }
        return isInRange; 
    }
}
