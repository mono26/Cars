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

    protected override void Awake()
    {
        base.Awake();
        if (movement == null) {
            GetComponent<EnemyMovement>();
        }
        if (stateMachine == null) {
            GetComponent<AIStateMachine>();
        }
        if (targetter == null) {
            GetComponent<Targetter>();
        }
        abilities = GetComponents<Ability>();
        return;
    }

    protected virtual void Start()
    {
        initialPosition = transform.position;
        stateMachine.ChangeState(startingState);
        updateStateRoutine = StartCoroutine(UpdateState());
        return;
    }

    private IEnumerator UpdateState()
    {
        stateMachine.UpdateState();
        yield return new WaitForSeconds(1 / stateUpdateRate);
        updateStateRoutine = StartCoroutine(UpdateState());
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

    /// <summary>
    /// Used to calculate a random patrol point inside the enemy influence zone. Return enemy position
    /// if there is no influence zone.
    /// </summary>
    /// <returns></returns>
    public Vector3 CalculateRandomPatrolPoint()
    {
        Vector3 patrolPoint = transform.position;
        try
        {
            if (HasTargetterComponent()) {
                patrolPoint = targetter.CalculateRandomPointInsideTrigger();
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return patrolPoint;
    }

    private bool HasTargetterComponent()
    {
        bool hasTargetter = true;
        if (targetter == null)
        {
            hasTargetter = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(Targetter));
        }
        return hasTargetter;
    }

    public void PatrolTowardsPoint(Vector3 _destinationPoint)
    {
        try
        {
            if (HasMovementComponent() && !movement.IsAlreadyInAPath()) {
                movement.SetNavigationDestination(_destinationPoint);
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    private bool HasMovementComponent()
    {
        bool hasMovement = true;
        if (movement == null)
        {
            hasMovement = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(EnemyMovement));
        }
        return hasMovement;
    }

    public void RunTowardsPoint(Vector3 _destinationPoint)
    {
        try
        {
            if (HasMovementComponent())
                movement.SetNavigationDestination(_destinationPoint);
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    public void StartRunning()
    {
        try
        {
            if (HasMovementComponent() && movement.GetCurrentMode != EnemyMovement.MovementMode.Running)
                movement.SetMovementValues(
                    stats.MovementStats.RunningAcceleration,
                    stats.MovementStats.RunningSpeed,
                    EnemyMovement.MovementMode.Running
                    );
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    public void StartPatrolling()
    {
        try
        {
            if (HasMovementComponent() && movement.GetCurrentMode != EnemyMovement.MovementMode.Patrolling)
                movement.SetMovementValues(
                    stats.MovementStats.WalkingAcceleration,
                    stats.MovementStats.WalkingSpeed,
                    EnemyMovement.MovementMode.Patrolling
                    );
        }
        catch (MissingComponentException missingComponentException)
        {
            missingComponentException.DisplayException();
        }
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

    /// <summary>
    /// The next ability to cast.
    /// </summary>
    /// <param name="_nextAbility"> The next ability to cast, can be NULL.</param>
    public void SetNextAbility(Ability _nextAbility)
    {
        nextAbility = _nextAbility;
        return;
    }

    public void ChangeState(AIState _stateToChangeTo)
    {
        if (HasStateMachineComponent()) {
            stateMachine.ChangeState(_stateToChangeTo);
        }
        return;
    }

    public bool HasStateMachineComponent()
    {
        bool hasStatemachine = true;
        if (stateMachine == null)
        {
            hasStatemachine = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(AIStateMachine));
        }
        return hasStatemachine;
    }

    public void ActivateNavigation(bool _value)
    {
        movement.NavigationSetActive(_value);
        return;
    }
}
