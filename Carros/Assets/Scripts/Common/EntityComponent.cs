using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    [SerializeField]
    protected Entity entity;

    protected virtual void Awake()
    {
        if (entity == null)
            entity = GetComponent<Entity>();

        return;
    }

    public virtual void EveryFrame()
    {
        HandleInput();

        return;
    }

    public virtual void FixedFrame()
    {

    }

    public virtual void LateFrame()
    {

    }

    protected virtual void HandleInput()
    {

    }
}
