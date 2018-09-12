using UnityEngine;

public abstract class EntityComponent : MonoBehaviour
{
    [Header("Entity Component settings")]
    [SerializeField] protected Entity entity = null;

    protected virtual void Awake()
    {
        if (entity == null)
            entity = GetComponent<Entity>();
        if (entity == null)
            entity = GetComponentInParent<Entity>();

        return;
    }

    public virtual void EveryFrame()
    {

    }

    public virtual void FixedFrame()
    {

    }

    public virtual void LateFrame()
    {

    }

    protected virtual void HandleAnimation()
    {

    }
}
