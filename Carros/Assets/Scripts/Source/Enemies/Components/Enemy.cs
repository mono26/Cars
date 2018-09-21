using System.Collections;
using UnityEngine;

public enum EnemyStateInTheWorld { Dead , Falling, Grounded }

[System.Serializable]
public class EnemyStats
{
    [SerializeField]
    private int maxHealth;
    public int MaxHealth { get { return maxHealth; } }
}

[RequireComponent(typeof(EnemyMovement), typeof(AIStateMachine), typeof(EnemyAnimationComponent))]
public class Enemy : Entity, EventHandler<TargetterEvent>
{
    [Header("Enemy settings")]
    [SerializeField] private AIState returnState;
    [SerializeField] private AIState startingState;
    [SerializeField] private EnemyStats stats; // Set in the editor. public EnemyStats Stats { get { return stats; } }
    [SerializeField] private float stateUpdateRate = 2.0f; // Updates per second
    [SerializeField] private float groundCheckRayLenght = 1.0f;

    [Header("Enemy components")]
    [SerializeField] private AIStateMachine aiStateMachineComponent;
    [SerializeField] private EnemyAnimationComponent enemyAnimationComponent;
    [SerializeField] private EnemyMovement enemyMovementComponent;
    [SerializeField] private Targetter targetterComponent;

    [Header("Enemy editor debugging")]
    [SerializeField] private Ability[] abilities;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Ability nextAbility;

    private Coroutine updateStateRoutine;

    public EnemyMovement GetMovementComponent { get { return enemyMovementComponent; } }
    public AIStateMachine GetStateMachineComponent { get { return aiStateMachineComponent; } }
    public Targetter GetTargetterComponent { get { return targetterComponent; } }
    public Ability[] GetAbilitiesComponent { get { return abilities; } }
    public Ability GetNextAbility { get { return nextAbility; } }
    public Vector3 GetInitialPosition { get { return initialPosition; } }

    protected override void Awake()
    {
        base.Awake();
        if (enemyMovementComponent == null) {
            GetComponent<EnemyMovement>();
        }
        if (aiStateMachineComponent == null) {
            GetComponent<AIStateMachine>();
        }
        if (targetterComponent == null) {
            GetComponent<Targetter>();
        }
        abilities = GetComponents<Ability>();
        return;
    }

    protected virtual void Start()
    {
        initialPosition = transform.position;
        if (HasStateMachineComponent())
        {
            aiStateMachineComponent.ChangeState(startingState);
            updateStateRoutine = StartCoroutine(UpdateState());
        }
        return;
    }

    private IEnumerator UpdateState()
    {
        aiStateMachineComponent.UpdateState();
        yield return new WaitForSeconds(1 / stateUpdateRate);
        updateStateRoutine = StartCoroutine(UpdateState());
        yield break;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<TargetterEvent>(this);
        return;
    }

    private void OnEnable()
    {
        EventManager.AddListener<TargetterEvent>(this);
        return;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleAnimations();
        return;
    }

    private void HandleAnimations()
    {
        EnemyStateInTheWorld currentStateInTheWorld;
        if (CheckIfIsGrounded()) {
            currentStateInTheWorld = EnemyStateInTheWorld.Grounded;
        }
        else {
            currentStateInTheWorld = EnemyStateInTheWorld.Falling;
        }
        EnemyMovemenState currentMovementState = GetMovementStateBaseOnSpeed();
        float speedPercent = CalculateMovementSpeedPercent();
        EnemyAnimationParameters enemyCurrentAnimationParameters = new EnemyAnimationParameters (
            currentStateInTheWorld,
            currentMovementState, 
            speedPercent);
        enemyAnimationComponent.HandleAnimations(enemyCurrentAnimationParameters);
    }

    private bool CheckIfIsGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * groundCheckRayLenght * 0.5f, -Vector3.up);
        bool isGrounded = Physics.Raycast(
            ray,
            out hit,
            groundCheckRayLenght,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
        return isGrounded;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        return;
    }

    private bool HasTargetterComponent()
    {
        bool hasTargetter = true;
        if (targetterComponent == null)
        {
            hasTargetter = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(Targetter));
        }
        return hasTargetter;
    }

    private EnemyMovemenState GetMovementStateBaseOnSpeed()
    {
        EnemyMovemenState currentMovementState = EnemyMovemenState.Idle;
        try
        {
            if (HasMovementComponent())
            {
                float currentSpeed = enemyMovementComponent.GetNavigationSpeed;
                MovementStats movementStats = enemyMovementComponent.GetMovementStats;
                if (currentSpeed <= movementStats.GetWalkingSpeed)
                {
                    currentMovementState = EnemyMovemenState.Walking;
                }
                else if (currentSpeed > movementStats.GetRunningSpeed)
                {
                    currentMovementState = EnemyMovemenState.Running;
                }
            }
        }
        catch (MissingComponentException missingComponentException){
            missingComponentException.DisplayException();
        }
        return currentMovementState;
    }

    private float CalculateMovementSpeedPercent()
    {
        float speeedPercent = 0;
        try
        {
            if (HasMovementComponent())
            {
                float currentSpeed = enemyMovementComponent.GetNavigationSpeed;
                MovementStats movementStats = enemyMovementComponent.GetMovementStats;
                if(currentSpeed <= movementStats.GetWalkingSpeed) {
                    speeedPercent = currentSpeed / movementStats.GetWalkingSpeed;
                }
                else {
                    speeedPercent = currentSpeed / movementStats.GetRunningSpeed;
                }
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return speeedPercent;
    }

    private bool HasMovementComponent()
    {
        bool hasMovement = true;
        if (enemyMovementComponent == null)
        {
            hasMovement = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(EnemyMovement));
        }
        return hasMovement;
    }

    private bool HasStateMachineComponent()
    {
        bool hasStatemachine = true;
        if (aiStateMachineComponent == null)
        {
            hasStatemachine = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(AIStateMachine));
        }
        return hasStatemachine;
    }

    private void StartPatrolling()
    {
        try
        {
            if (HasMovementComponent())
            {
                enemyMovementComponent.SetNavigationValuesDependingOnMode(EnemyMovementMode.Patrolling);
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    private void StartRunning()
    {
        try
        {
            if (HasMovementComponent())
            {
                enemyMovementComponent.SetNavigationValuesDependingOnMode(EnemyMovementMode.Running);
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    public void RunTowardsPoint(Vector3 _destinationPoint)
    {
        StartRunning();
        try
        {
            if (HasMovementComponent())
                enemyMovementComponent.SetNavigationDestination(_destinationPoint);
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    public void PatrolTowardsPoint(Vector3 _destinationPoint)
    {
        StartPatrolling();
        try
        {
            if (HasMovementComponent() && !enemyMovementComponent.IsAlreadyInAPath())
            {
                enemyMovementComponent.SetNavigationDestination(_destinationPoint);
            }
        }
        catch (MissingComponentException missingComponentException)
        {
            missingComponentException.DisplayException();
        }
        return;
    }

    public void ActivateNavigation(bool _value)
    {
        enemyMovementComponent.ActivateNavMeshNavigation(_value);
        return;
    }

    public bool CanAttackTarget()
    {
        bool canAttack = false;
        if(HasTargetterComponent() && targetterComponent is SlotTargetter)
        {
            SlotTargetter slotTargetter = targetterComponent as SlotTargetter;
            if(slotTargetter.HasAValidCurrentTarget() && slotTargetter.GetCurrentSlotType() == SlotType.Attacking) {
                canAttack = true;
            }
        }
        else {
            canAttack = targetterComponent.HasAValidCurrentTarget();
        }
        return canAttack;
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
            if (HasTargetterComponent())
            {
                patrolPoint = targetterComponent.CalculateRandomPointInsideTrigger();
            }
        }
        catch (MissingComponentException missingComponentException)
        {
            missingComponentException.DisplayException();
        }
        return patrolPoint;
    }

    public void ChangeState(AIState _stateToChangeTo)
    {
        if (HasStateMachineComponent())
        {
            aiStateMachineComponent.ChangeState(_stateToChangeTo);
        }
        return;
    }

    public Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = transform.position;
        if (HasTargetterComponent())
        {
            targetPosition = targetterComponent.GetCurrentTargetPosition();
        }
        return targetPosition;
    }

    public void OnEvent(TargetterEvent _targetterEvent)
    {
        if (!_targetterEvent.enemy.Equals(this)) { return; }

        switch (_targetterEvent.eventType)
        {
            case TargetterEventType.TargetLost:
                aiStateMachineComponent.ChangeState(returnState);
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

}
