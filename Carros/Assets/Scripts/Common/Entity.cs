using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType { Playable, AIControlled}

    [SerializeField]
    protected AudioSource audioSource;
    [SerializeField]
    protected Rigidbody body;
    public Rigidbody Body { get { return body; } }
    [SerializeField]
    protected ExternalInput input;
    public ExternalInput Input { get { return input; } }
    [SerializeField]
    protected Mesh mesh;
    [SerializeField]
    protected MeshCollider meshCollider;
    [SerializeField]
    protected EntityType type = EntityType.AIControlled;
    public EntityType Type { get { return type; } }

    protected EntityComponent[] components;

    protected virtual void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (body == null)
            body = GetComponent<Rigidbody>();
        if (meshCollider == null)
            meshCollider = GetComponent<MeshCollider>();
        if (mesh == null)
            mesh = GetComponent<Mesh>();

        components = GetComponents<EntityComponent>();

        return;
    }

    protected virtual void Update()
    {
        if (components != null)
        {
            foreach (EntityComponent component in components)
            {
                component.EveryFrame();
            }
        }

        return;
    }

    protected virtual void FixedUpdate()
    {
        if (components != null)
        {
            foreach (EntityComponent component in components)
            {
                component.FixedFrame();
            }
        }

        return;
    }

    protected virtual void LateUpdate()
    {
        if (components != null)
        {
            foreach (EntityComponent component in components)
            {
                component.LateFrame();
            }
        }

        return;
    }
}
