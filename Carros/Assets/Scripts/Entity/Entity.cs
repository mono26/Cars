using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType { Playable, AIControlled}

    [Header("Entity settings")]
    [SerializeField] protected EntityType type = EntityType.AIControlled;

    [Header("Entity components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected Rigidbody body;
    [SerializeField] protected Collider[] hitBox;
    [SerializeField] protected ExternalInput input;
    [SerializeField] protected SkinnedMeshRenderer[] model;

    [Header("Editor debugging")]
    [SerializeField]
    protected EntityComponent[] components;

    public Animator GetAnimator { get { return animator; } }
    public Rigidbody GetBody { get { return body; } }
    public T GetControlledEntity<T>() where T : Entity { { return this as T; } }

    protected virtual void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (body == null)
            body = GetComponent<Rigidbody>();
        if (hitBox == null)
            hitBox = GetComponentsInChildren<Collider>();
        if (model == null)
            model = GetComponentsInChildren<SkinnedMeshRenderer>();

        components = GetComponentsInChildren<EntityComponent>();

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
