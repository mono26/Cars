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
    [SerializeField]
    protected float groundCheckRayLenght = 1.0f;

    [Header("Enemy Movement Components")]
    [SerializeField]
    protected NavMeshAgent navigation;
    public NavMeshAgent Navigation { get { return navigation; } }

    [Header("Editor debugging")]
    [SerializeField]
    protected MovementMode currentMode;
    public MovementMode CurrentMode { get { return currentMode; } }
    protected Vector3 lastPosition;

    protected override void Awake()
    {
        base.Awake();

        if (navigation == null)
            GetComponent<NavMeshAgent>();

        return;
    }

    protected bool CheckGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * groundCheckRayLenght * 0.5f, -Vector3.up);
        bool isGrounded = Physics.Raycast(ray, out hit, groundCheckRayLenght, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);

        return isGrounded;
    }

    protected bool CheckIfStandingStill()
    {
        bool isStandingStill = navigation.desiredVelocity.sqrMagnitude <= 0.1f * 0.1f ? true : false;
        if (isStandingStill)
            currentMode = MovementMode.Idle;

        return isStandingStill;
    }

    public override void FixedFrame()
    {
        HandleAnimations();

        return;
    }

    private void HandleAnimations()
    {
        if (entity.Animator == null) { return; }

        HandleAnimationStates();

        HandleAnimationSpeed();

        lastPosition = transform.position;

        return;
    }

    protected void HandleAnimationSpeed()
    {
        Animator animatorToHandle = entity.Animator;
        if (animatorToHandle == null || navigation == null) { return; }

        if (currentMode == MovementMode.Idle) { animatorToHandle.speed = 1.0f; }
        else
        {
            float currentSpeed = navigation.velocity.magnitude;
            if(currentSpeed < 1.0f) { animatorToHandle.speed = currentSpeed; }
            else
            {
                if (navigation.height < 1.0f) { animatorToHandle.speed = navigation.velocity.magnitude / (navigation.height + 1); }
                else { animatorToHandle.speed = navigation.velocity.magnitude / (navigation.height + navigation.height); }
            }
        }

        return;
    }

    protected void HandleAnimationStates()
    {
        Animator animatorToHandle = entity.Animator;
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

    public void NavigationSetActive(bool _state)
    {
        if (navigation == null) { return; }

        if (!_state && navigation.enabled)
            navigation.ResetPath();

        navigation.enabled = _state;

        return;
    }

    protected void OnAnimatorMove()
    {
        //navigation.speed = (entity.Animator.deltaPosition / Time.deltaTime).magnitude;
        transform.position = navigation.nextPosition;

        return;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))
            NavigationSetActive(true);

        return;
    }

    /// <summary>
    /// Used to set the enemy destination
    /// </summary>
    /// <param name="_destination"> The destination for the navMeshAgent.</param>
    public void SetNavigationDestination(Vector3 _destination)
    {
        if (navigation == null) { return; }

        if (navigation.isOnNavMesh)
            navigation.destination = _destination;
        //navigation.SetDestination(_destination);

        return;
    }

    public void SetMovementValues(float _acceleration, float _speed, MovementMode _mode)
    {
        if (navigation == null) { return; }

        currentMode = _mode;
        navigation.acceleration = _acceleration;
        navigation.speed = _speed;

        return;
    }

    protected void Start()
    {
        navigation.updatePosition = false;

        return;
    }
}
