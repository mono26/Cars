using UnityEngine;

public class Ability : EntityComponent
{
    [Header("Ability settings")]
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected float range;
    public float Range { get { return range; } }

    protected float lastCast = 0;

    protected virtual void Start()
    {
        lastCast = Time.timeSinceLevelLoad - cooldown;

        return;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(entity.transform.position, range);

        return;
    }

    public virtual void Cast()
    {
        lastCast = Time.timeSinceLevelLoad;

        return;
    }

    public bool IsInRange()
    {
        bool isInRange = false;
        if (Vector3.Distance(entity.transform.position, entity.Target.position) <= range)
            isInRange = true;

        return isInRange; 
    }

    public bool IsInCooldown()
    {
        bool isInCooldown = true;
        if (Time.timeSinceLevelLoad > lastCast + cooldown)
            isInCooldown = false;

        return isInCooldown;
    }
}
