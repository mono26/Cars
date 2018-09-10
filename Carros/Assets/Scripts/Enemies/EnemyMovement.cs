using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementEvent : CarEvent
{
    public Enemy enemy;
    public EnemyMovement.MovementMode movementType;

    public EnemyMovementEvent(Enemy _enemy, EnemyMovement.MovementMode _movementType)
    {
        enemy = _enemy;
        movementType = _movementType;
    }
}

public class EnemyMovement : EntityComponent
{
    public enum MovementMode { Running, Patrolling, Idle }

    [System.Serializable]
    public class MovementStats
    {
        [SerializeField]
        protected float runningAcceleration;
        public float RunningAcceleration { get { return runningAcceleration; } }
        [SerializeField]
        protected float runningSpeed;
        public float RunningSpeed { get { return runningSpeed; } }
        [SerializeField]
        protected float walkingAcceleration;
        public float WalkingAcceleration { get { return walkingAcceleration; } }
        [SerializeField]
        protected float walkingSpeed;
        public float WalkingSpeed { get { return walkingSpeed; } }

        public MovementStats(float _runningAcceleration, float _runningSpeed, float _walkingAcceleration, float _walkingSpeed)
        {
            runningAcceleration = _runningAcceleration;
            runningSpeed = _runningSpeed;
            walkingAcceleration = _walkingAcceleration;
            walkingSpeed = _walkingSpeed;
        }
    }

    [Header("Enemy Movement settings")]
    [SerializeField] private float groundCheckRayLenght = 1.0f;

    [Header("Enemy Movement Components")]
    [SerializeField] private NavMeshAgent navigationComponent;

    [Header("Editor debugging")]
    [SerializeField] private MovementMode currentMode;

    protected Vector3 lastPosition;

    public MovementMode GetCurrentMode { get { return currentMode; } }
    public NavMeshAgent GetNavigationComponent { get { return navigationComponent; } }

    protected override void Awake()
    {
        if (navigationComponent == null) {
            GetComponent<NavMeshAgent>();
        }
        base.Awake();
        return;
    }

    private void Start()
    {
        navigationComponent.updatePosition = false;
        return;
    }

    public override void FixedFrame()
    {
        HandleAnimations();
        return;
    }

    private void OnAnimatorMove()
    {
        //Debug.Log(Time.timeSinceLevelLoad + "Navigation next position: " + navigation.nextPosition);
        if (HasNavigationComponent()) {
            transform.position = navigationComponent.nextPosition;
        }
        return;
    }

    private void HandleAnimations()
    {
        if (entity.GetAnimatorComponent == null) { return; }
        HandleAnimationStates();
        HandleAnimationSpeed();
        lastPosition = transform.position;
        return;
    }

    private void HandleAnimationStates()
    {
        Animator animatorToHandle = entity.GetAnimatorComponent;
        if (animatorToHandle == null) { return; }
        animatorToHandle.SetBoolWithParameterCheck(
            "IsGrounded",
            AnimatorControllerParameterType.Bool,
            CheckGrounded()
            );
        animatorToHandle.SetBoolWithParameterCheck(
            "IsIdle",
            AnimatorControllerParameterType.Bool,
            CheckIfStandingStill()
            );
        animatorToHandle.SetBoolWithParameterCheck(
            "IsRunning",
            AnimatorControllerParameterType.Bool,
            (currentMode == MovementMode.Running)
            );
        animatorToHandle.SetBoolWithParameterCheck(
            "IsWalking",
            AnimatorControllerParameterType.Bool,
            (currentMode == MovementMode.Patrolling)
            );
        return;
    }

    private bool CheckGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * groundCheckRayLenght * 0.5f, -Vector3.up);
        bool isGrounded = Physics.Raycast (ray, out hit, groundCheckRayLenght, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
        return isGrounded;
    }

    private bool CheckIfStandingStill()
    {
        bool isStandingStill = true;
        try
        {
            if (HasNavigationComponent())
            {
                isStandingStill = navigationComponent.desiredVelocity.sqrMagnitude <= 0.1f * 0.1f ? true : false;
                if (isStandingStill)
                    currentMode = MovementMode.Idle;
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return isStandingStill;
    }

    private bool HasNavigationComponent()
    {
        bool hasNavigation = true;
        if (navigationComponent == null)
        {
            hasNavigation = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(NavMeshAgent));
        }
        return hasNavigation;
    }

    private void HandleAnimationSpeed()
    {
        try
        {
            Animator animatorToHandle = entity.GetAnimatorComponent;
            if (animatorToHandle != null && HasNavigationComponent())
            {
                if (currentMode == MovementMode.Idle) { animatorToHandle.speed = 1.0f; }
                else
                {
                    float currentSpeed = navigationComponent.velocity.magnitude;
                    if (currentSpeed < 1.0f) { animatorToHandle.speed = currentSpeed; }
                    else
                    {
                        if (navigationComponent.height < 1.0f) { animatorToHandle.speed = navigationComponent.velocity.magnitude / (navigationComponent.height + 1); }
                        else { animatorToHandle.speed = navigationComponent.velocity.magnitude / (navigationComponent.height + navigationComponent.height); }
                    }
                }
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))
        {
            NavigationSetActive(true);
            entity.BodyAffectedByGravity(false);
        }
        return;
    }

    public void NavigationSetActive(bool _state)
    {
        try
        {
            if (HasNavigationComponent())
            {
                if (!_state && navigationComponent.enabled)
                    navigationComponent.ResetPath();
                navigationComponent.enabled = _state;
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    /// <summary>
    /// Used to set the enemy destination
    /// </summary>
    /// <param name="_destination"> The destination for the navMeshAgent.</param>
    public void SetNavigationDestination(Vector3 _destination)
    {
        try
        {
            if (HasNavigationComponent() && navigationComponent.isOnNavMesh) {
                navigationComponent.destination = _destination;
                Debug.Log(Time.timeSinceLevelLoad + "New navigation destination: " + navigationComponent.destination);
            }

        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    public void SetMovementValues(float _acceleration, float _speed, MovementMode _mode)
    {
        if (navigationComponent == null) { return; }
        currentMode = _mode;
        navigationComponent.acceleration = _acceleration;
        navigationComponent.speed = _speed;
        return;
    }

    public bool IsAlreadyInAPath()
    {
        bool inPath = false;
        try
        {
            if(HasNavigationComponent())
                inPath = navigationComponent.hasPath;
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return inPath;
    }
}
