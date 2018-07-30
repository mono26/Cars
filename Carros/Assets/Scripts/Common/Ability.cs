using UnityEngine;

public abstract class Ability : EntityComponent
{
    [Header("Ability settings")]
    [SerializeField]
    protected float range;
    public float Range { get { return range; } }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(entity.transform.position, range);

        return;
    }

    public abstract void Cast();

    public bool IsInRange()
    {
        bool isInRange = false;
        if (Vector3.Distance(entity.transform.position, entity.Target.position) <= range)
            isInRange = true;

        return isInRange; 
    }
}
