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

public class EnemyMovement : AIEntityComponent
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
    protected AudioClip backStep;
    [SerializeField]
    protected AudioClip frontStep;
    [SerializeField]
    protected float groundCheckRayLenght = 0.5f;

    [Header("Enemy Movement Components")]
    [SerializeField]
    protected NavMeshAgent navigation;
    public NavMeshAgent Navigation { get { return navigation; } }

    [Header("Editor debugging")]
    [SerializeField]
    protected MovementMode currentMode;
    public MovementMode CurrentMode { get { return currentMode; } }

    public void SetFollowNavigation(bool _state)
    {
        if(navigation == null) { return; }

        if (_state)
            navigation.enabled = true;
        else if (!_state)
            navigation.enabled = false;

        return;
    }

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

        entity.Animator.SetBoolWithParameterCheck(
            "IsGrounded",
            AnimatorControllerParameterType.Bool,
            CheckGrounded()
            );

        entity.Animator.SetBoolWithParameterCheck(
            "IsIdle",
            AnimatorControllerParameterType.Bool,
            CheckIfStandingStill()
            );

        entity.Animator.SetBoolWithParameterCheck(
            "IsRunning",
            AnimatorControllerParameterType.Bool,
            (currentMode == MovementMode.Running)
            );

        entity.Animator.SetBoolWithParameterCheck(
            "IsWalking",
            AnimatorControllerParameterType.Bool,
            (currentMode == MovementMode.Patrolling)
            );

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
            SetFollowNavigation(true);

        return;
    }

    protected void PlayStep(int frontFoot)
    {
        /*if (frontStepAudio != null && frontFoot == 1)
            frontStepAudio.PlayRandomClip();
        else if (backStepAudio != null && frontFoot == 0)
            backStepAudio.PlayRandomClip();*/

        return;
    }

    /// <summary>
    /// Used to set the enemy destination
    /// </summary>
    /// <param name="_destination"> The destination for the navMeshAgent.</param>
    public void SetMovementDestination(Vector3 _destination)
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
