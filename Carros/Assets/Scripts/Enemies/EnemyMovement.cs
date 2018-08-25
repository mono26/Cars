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

public class EnemyMovement : AIEntityComponent, EventHandler<EnemyMovementEvent>
{
    public enum MovementMode { Running, Patrolling }

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
    [SerializeField]
    protected bool isGrounded;

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

    protected void CheckGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * groundCheckRayLenght * 0.5f, -Vector3.up);
        isGrounded = Physics.Raycast(ray, out hit, groundCheckRayLenght, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);

        return;
    }

    public override void FixedFrame()
    {
        CheckGrounded();

        if(entity.Animator == null) { return; }

        entity.Animator.SetBoolWithParameterCheck(
            "IsGrounded",
            AnimatorControllerParameterType.Bool,
            isGrounded
            );
    }

    protected void OnAnimatorMove()
    {
        navigation.speed = (entity.Animator.deltaPosition / Time.deltaTime).magnitude;
        transform.position = navigation.nextPosition;

        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<EnemyMovementEvent>(this);

        return;
    }

    protected void OnEnable()
    {
        EventManager.AddListener<EnemyMovementEvent>(this);

        return;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))
            SetFollowNavigation(true);

        return;
    }

    public void OnEvent(EnemyMovementEvent _movementEvent)
    {
        if (!_movementEvent.enemy.Equals(aiEntity)) { return; }

        switch (_movementEvent.movementType)
        {
            case MovementMode.Running:
                StopPatrolling();
                StartChassing();
                break;

            case MovementMode.Patrolling:
                StopChassing();
                StartPatrolling();
                break;

            default:
                break;
        }
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

    protected void StartPatrolling()
    {
        if (currentMode != EnemyMovement.MovementMode.Patrolling)
            SetMovementValues(
                aiEntity.Stats.MovementStats.RunningSpeed,
                aiEntity.Stats.MovementStats.RunningAcceleration,
                EnemyMovement.MovementMode.Running
                );

        if (entity.Animator == null) { return; }

        entity.Animator.SetBoolWithParameterCheck(
                "IsWalking",
                AnimatorControllerParameterType.Bool,
                true
                );

        return;
    }

    protected void StartChassing()
    {
        if (currentMode != EnemyMovement.MovementMode.Patrolling)
            SetMovementValues(
                aiEntity.Stats.MovementStats.WalkingAcceleration,
                aiEntity.Stats.MovementStats.WalkingSpeed,
                EnemyMovement.MovementMode.Patrolling
                );

        if (entity.Animator == null) { return; }

        entity.Animator.SetBoolWithParameterCheck(
            "IsRunning",
            AnimatorControllerParameterType.Bool,
            true
            );

        return;
    }

    protected void StopChassing()
    {
        if (entity.Animator == null) { return; }

        entity.Animator.SetBoolWithParameterCheck(
            "IsRunning",
            AnimatorControllerParameterType.Bool,
            false
            );

        return;
    }

    protected void StopPatrolling()
    {
        entity.Animator.SetBoolWithParameterCheck(
            "IsWalking",
            AnimatorControllerParameterType.Bool,
            false
            );

        return;
    }
}
