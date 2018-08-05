using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType { Playable, AIControlled}

    [Header("Entity components")]
    [SerializeField]
    protected AudioSource audioSource;
    [SerializeField]
    protected Rigidbody body;
    public Rigidbody Body { get { return body; } }
    [SerializeField]
    protected Collider[] hitBox;
    [SerializeField]
    protected ExternalInput input;
    public ExternalInput Input { get { return input; } }
    [SerializeField]
    protected SkinnedMeshRenderer model;

    [SerializeField]
    protected EntityType type = EntityType.AIControlled;
    public EntityType Type { get { return type; } }

    [Header("Editor debugging")]
    protected EntityComponent[] components;

    protected virtual void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (body == null)
            body = GetComponent<Rigidbody>();
        if (hitBox == null)
            hitBox = transform.GetComponentsInChildren<Collider>();
        if (model == null)
            model = GetComponent<SkinnedMeshRenderer>();

        components = GetComponents<EntityComponent>();

        return;
    }

    protected virtual void Update()
    {
        if (components == null) { return; }

        foreach (EntityComponent component in components)
        {
            component.EveryFrame();
        }

        return;
    }

    protected virtual void FixedUpdate()
    {
        if (components == null) { return; }

        foreach (EntityComponent component in components)
        {
            component.FixedFrame();
        }

        return;
    }

    protected virtual void LateUpdate()
    {
        if (components == null) { return; }

        foreach (EntityComponent component in components)
        {
            component.LateFrame();
        }

        return;
    }
}
