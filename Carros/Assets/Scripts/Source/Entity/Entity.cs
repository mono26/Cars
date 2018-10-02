using UnityEngine;

public enum EntityType { Playable, AIControlled }

// Empty class for al the posible entity inputs that we could need: car, enemies, etc.
public class EntityInput
{

}

public class Entity : MonoBehaviour
{
    [Header("Entity settings")]
    [SerializeField] private EntityType entityType = EntityType.AIControlled;

    [Header("Entity components")]
    [SerializeField] private Animator animatorComponent;
    [SerializeField] private AudioSource audioComponent;
    [SerializeField] private Rigidbody bodyComponent;
    [SerializeField] private Collider[] hitBox;
    [SerializeField] private SkinnedMeshRenderer[] model;

    [Header("Entity editor debugging")]
    [SerializeField]
    protected EntityComponent[] components;

    public Animator GetAnimatorComponent { get { return animatorComponent; } }
    public Rigidbody GetBody { get { return bodyComponent; } }
    public EntityType GetEntityType { get { return entityType; } }

    protected virtual void Awake()
    {
        if(animatorComponent == null) {
            animatorComponent = GetComponent<Animator>();
        }
        if (audioComponent == null) {
            audioComponent = GetComponent<AudioSource>();
        }
        if (bodyComponent == null) {
            bodyComponent = GetComponent<Rigidbody>();
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
        if (HasEntitytComponents())
        {
            foreach (EntityComponent component in components) {
                component.EveryFrame();
            }
        }
        return;
    }

    private bool HasEntitytComponents()
    {
        bool hasComponents = true;
        try
        {
            if (components == null || components.Length == 0)
            {
                hasComponents = false;
                throw new MissingComponentException(gameObject, typeof(EntityComponent));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasComponents;
    }

    protected virtual void FixedUpdate()
    {
        if (HasEntitytComponents())
        {
            foreach (EntityComponent component in components) {
                component.FixedFrame();
            }
        }
        return;
    }

    protected virtual void LateUpdate()
    {
        if (HasEntitytComponents())
        {
            foreach (EntityComponent component in components) {
                component.LateFrame();
            }
        }
        return;
    }

    public virtual void ReceiveInput(EntityInput _inputToReceive)
    {

    }

    public void SetIfBodyAffectedByGravity(bool _value)
    {
        bodyComponent.useGravity = _value;
        return;
    }

    /// <summary>
    /// Applies a raw velocity vector. The method doesn't normalize the vector.
    /// </summary>
    /// <param name="_newVelocity"> New velocity for the body.</param>
    public void SetBodyVelocity(Vector3 _newVelocity)
    {
        bodyComponent.velocity = _newVelocity;
        return;
    }
}
