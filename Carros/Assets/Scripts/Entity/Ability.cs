using UnityEngine;

public abstract class Ability : EntityComponent
{
    [Header("Ability settings")]
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected float range;
    public float Range { get { return range; } }

    protected float lastCast = 0;

    public virtual void Cast()
    {
        lastCast = Time.timeSinceLevelLoad;

        return;
    }

    public bool IsInCooldown()
    {
        bool isInCooldown = true;
        if (Time.timeSinceLevelLoad > lastCast + cooldown)
            isInCooldown = false;

        return isInCooldown;
    }

    public bool IsInRange(Transform _target)
    {
        bool isInRange = false;

        float distance = Vector3.Distance(
            entity.transform.position, 
            _target.position
            );
        if (distance <= range)
            isInRange = true;

        return isInRange; 
    }

    public bool IsInRange(SlotManager _slotTarget)
    {
        bool isInRange = false;

        float distance = Vector3.Distance(
            entity.transform.position,
            _slotTarget.transform.position
            );
        if (distance - _slotTarget.AttackingDistaceToEntity <= range)
            isInRange = true;

        return isInRange;
    }

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
}
