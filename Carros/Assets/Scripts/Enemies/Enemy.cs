using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(AIStateMachine))]
public class Enemy : Entity, EventHandler<TargetterEvent>
{
    [System.Serializable]
    public class EnemyStats
    {
        [SerializeField]
        protected EnemyMovement.MovementStats movementStats;
        public EnemyMovement.MovementStats MovementStats { get { return movementStats; } }
        [SerializeField]
        protected float maxHealth;
        public float MaxHealth { get { return maxHealth; } }
    }

    [Header("Enemy settings")]
    protected AudioClip idleAudio;
    [SerializeField]
    protected AIState returnState;
    [SerializeField]
    protected AIState startingState;
    [SerializeField]
    protected EnemyStats stats; // Set in the editor.
    public EnemyStats Stats { get { return stats; } }
    [SerializeField]
    protected float stateUpdateRate = 2.0f; // Updates per second

    [Header("Enemy components")]
    [SerializeField]
    protected EnemyMovement movement;
    public EnemyMovement Movement { get { return movement; } }
    [SerializeField]
    protected AIStateMachine stateMachine;
    public AIStateMachine StateMachine { get { return stateMachine; } }
    [SerializeField]
    protected Targetter targetter;
    public Targetter Targetter { get { return targetter; } }

    [Header("Editor debugging")]
    [SerializeField]
    protected Ability[] abilities;
    public Ability[] Abilities { get { return abilities; } }
    [SerializeField]
    protected Vector3 initialPosition;
    public Vector3 InitialPosition { get { return initialPosition; } }
    [SerializeField]
    protected Ability nextAbility;
    public Ability NextAbility { get { return nextAbility; } }
    [SerializeField]
    protected Coroutine updateStateRoutine;

    /// <summary>
    /// The next ability to cast.
    /// </summary>
    /// <param name="_nextAbility"> The next ability to cast, can be NULL.</param>
    public void SetNextAbility(Ability _nextAbility) { nextAbility = _nextAbility; }

    protected override void Awake()
    {
        base.Awake();

        if (movement == null)
            GetComponent<EnemyMovement>();
        if (stateMachine == null)
            GetComponent<AIStateMachine>();
        if (targetter == null)
            GetComponent<Targetter>();

        abilities = GetComponents<Ability>();

        return;
    }

    public void Grunt()
    {
        /*if (gruntAudio != null)
            gruntAudio.PlayRandomClip();*/

        return;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        return;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<TargetterEvent>(this);

        return;
    }

    protected void OnEnable()
    {
        EventManager.AddListener<TargetterEvent>(this);

        return;
    }

    public void OnEvent(TargetterEvent _targetterEvent)
    {
        if (!_targetterEvent.enemy.Equals(this)) { return; }

        switch (_targetterEvent.eventType)
        {
            case TargetterEventType.TargetLost:
                stateMachine.ChangeState(returnState);
                break;

            default:
                break;
        }
    }

    protected virtual void Start()
    {
        initialPosition = transform.position;

        movement.SetMovementValues(
            stats.MovementStats.RunningSpeed,
            stats.MovementStats.RunningAcceleration,
            EnemyMovement.MovementMode.Running
            );

        stateMachine.ChangeState(startingState);
        updateStateRoutine = StartCoroutine(UpdateState());

        return;
    }

    protected IEnumerator UpdateState()
    {
        stateMachine.UpdateState();

        yield return new WaitForSeconds(1 / stateUpdateRate);

        updateStateRoutine = StartCoroutine(UpdateState());
    }
}
