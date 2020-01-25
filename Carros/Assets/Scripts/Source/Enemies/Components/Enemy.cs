// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

public enum EnemyStateInTheWorld { Dead , Falling, Grounded }

[RequireComponent(typeof(EnemyMovement), typeof(AIStateMachine), typeof(EnemyAnimationComponent))]
public class Enemy : Entity, EventHandler<TargetterEvent>
{
    [Header("Enemy settings")]
    [SerializeField] private AIState returnState;
    [SerializeField] private AIState startingState;
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

    private bool HasStateMachineComponent()
    {
        bool hasStatemachine = true;
        try
        {
            if (aiStateMachineComponent == null)
            {
                hasStatemachine = false;
                throw new MissingComponentException(gameObject, typeof(AIStateMachine));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasStatemachine;
    }

    private IEnumerator UpdateState()
    {
        aiStateMachineComponent.UpdateState();
        yield return new WaitForSeconds(1 / stateUpdateRate);
        updateStateRoutine = StartCoroutine(UpdateState());
        yield break;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        HandleAnimations();
        return;
    }

    private void OnEnable()
    {
        EventManager.AddListener<TargetterEvent>(this);
        return;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<TargetterEvent>(this);
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

    private bool HasTargetterComponent()
    {
        bool hasTargetter = true;
        try
        {
            if (targetterComponent == null)
            {
                hasTargetter = false;
                throw new MissingComponentException(gameObject, typeof(Targetter));
            }
        }
        catch (MissingComponentException missingComponentException)
        {
            missingComponentException.DisplayException();
        }
        return hasTargetter;
    }

    private EnemyMovemenState GetMovementStateBaseOnSpeed()
    {
        EnemyMovemenState currentMovementState = EnemyMovemenState.Idle;
        if (HasMovementComponent())
        {
            float currentSpeed = enemyMovementComponent.GetNavigationSpeed;
            MovementStats movementStats = enemyMovementComponent.GetMovementStats;
            if (currentSpeed <= movementStats.GetWalkingSpeed) {
                currentMovementState = EnemyMovemenState.Walking;
            }
            else if (currentSpeed > movementStats.GetRunningSpeed) {
                currentMovementState = EnemyMovemenState.Running;
            }
        }
        return currentMovementState;
    }

    private float CalculateMovementSpeedPercent()
    {
        float speeedPercent = 0;
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
        return speeedPercent;
    }

    private bool HasMovementComponent()
    {
        bool hasMovement = true;
        try
        {
            if (enemyMovementComponent == null)
            {
                hasMovement = false;
                throw new MissingComponentException(gameObject, typeof(EnemyMovement));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasMovement;
    }

    private void StartPatrolling()
    {
        if (HasMovementComponent())
        {
            enemyMovementComponent.SetNavigationValuesDependingOnMode(EnemyMovementMode.Patrolling);
        }
        return;
    }

    private void StartRunning()
    {
        if (HasMovementComponent())
        {
            enemyMovementComponent.SetNavigationValuesDependingOnMode(EnemyMovementMode.Running);
        }
        return;
    }

    public void RunTowardsPoint(Vector3 _destinationPoint)
    {
        StartRunning();
        if (HasMovementComponent()) {
            enemyMovementComponent.SetNavigationDestination(_destinationPoint);
        }
        return;
    }

    public void PatrolTowardsPoint(Vector3 _destinationPoint)
    {
        StartPatrolling();
        if (HasMovementComponent() && !enemyMovementComponent.IsAlreadyInAPath()) {
            enemyMovementComponent.SetNavigationDestination(_destinationPoint);
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
        if(targetterComponent is SlotTargetter)
        {
            SlotTargetter slotTargetter = targetterComponent as SlotTargetter;
            if(HasValidTarget() && slotTargetter.GetCurrentSlotType() == SlotType.Attacking) {
                canAttack = true;
            }
        }
        else {
            canAttack = HasValidTarget();
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
        if (HasTargetterComponent())
        {
            patrolPoint = targetterComponent.CalculateRandomPointInsideTrigger();
        }
        return patrolPoint;
    }

    public void ChangeState(AIState _stateToChangeTo)
    {
        if (HasStateMachineComponent()) {
            aiStateMachineComponent.ChangeState(_stateToChangeTo);
        }
        return;
    }

    public Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = transform.position;
        if (HasTargetterComponent()) {
            targetPosition = targetterComponent.GetCurrentTargetPosition();
        }
        return targetPosition;
    }

    public bool HasValidTarget()
    {
        bool canAttack = false;
        if (HasTargetterComponent()) {
            canAttack = targetterComponent.HasAValidCurrentTarget();
        }
        return canAttack;
    }

    public void OnGameEvent(TargetterEvent _targetterEvent)
    {
        if (_targetterEvent.GetEnemy.Equals(this))
        {
            switch (_targetterEvent.GetEventType)
            {
                case TargetterEventType.TargetLost:
                    ChangeState(returnState);
                    break;
                default:
                    break;
            }
        }
        return;
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
