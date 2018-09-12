using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum EntityType { Playable, AIControlled}

    [Header("Entity settings")]
    [SerializeField] protected EntityType type = EntityType.AIControlled;

    [Header("Entity components")]
    [SerializeField] private Animator animatorComponent;
    [SerializeField] private AudioSource audioComponent;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Collider[] hitBox;
    [SerializeField] private ExternalInput inputcomponent;
    [SerializeField] private SkinnedMeshRenderer[] model;

    [Header("Editor debugging")]
    [SerializeField]
    protected EntityComponent[] components;

    public Animator GetAnimatorComponent { get { return animatorComponent; } }
    public Rigidbody GetBody { get { return body; } }
    public T GetControlledEntity<T>() where T : Entity { { return this as T; } }
    public ExternalInput GetInputcomponent { get { return inputcomponent; } }

    protected virtual void Awake()
    {
        if (audioComponent == null) {
            audioComponent = GetComponent<AudioSource>();
        }
        if (body == null) {
            body = GetComponent<Rigidbody>();
        }
        if (hitBox == null) {
            hitBox = GetComponentsInChildren<Collider>();
        }
        if (model == null) {
            model = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        components = GetComponentsInChildren<EntityComponent>();
        return;
    }

    protected virtual void Update()
    {
        if (components == null) { return; }
        foreach (EntityComponent component in components) {
            component.EveryFrame();
        }
        return;
    }

    protected virtual void FixedUpdate()
    {
        if (components == null) { return; }
        foreach (EntityComponent component in components) {
            component.FixedFrame();
        }
        return;
    }

    protected virtual void LateUpdate()
    {
        if (components == null) { return; }
        foreach (EntityComponent component in components) {
            component.LateFrame();
        }
        return;
    }

    protected bool CanApplyExternalInputToEntity()
    {
        bool canApply = false;
        canApply = (type == EntityType.Playable && inputcomponent != null) ? true : false;
        return canApply;
    }

    /// <summary>
    /// Applies raw velocity vector. The method doesn't normalize the velocity.
    /// </summary>
    /// <param name="_newVelocity"> New velocity for the body.</param>
    public void SetBodyVelocity(Vector3 _newVelocity)
    {
        body.velocity = _newVelocity;
        return;
    }

    public void BodyAffectedByGravity(bool _value)
    {
        body.useGravity = _value;
        return;
    }
}
