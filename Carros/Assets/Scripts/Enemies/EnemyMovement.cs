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
    public enum MovementMode { Running, Walking }

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

        public MovementStats(float _rA, float _rS, float _wA, float _wS)
        {
            runningAcceleration = _rA;
            runningSpeed = _rS;
            walkingAcceleration = _wA;
            walkingSpeed = _wS;
        }
    }

    [Header("Enemy Movement Components")]
    [SerializeField]
    protected NavMeshAgent navigation;
    public NavMeshAgent Navigation { get { return navigation; } }

    [Header("Editor debugging")]
    [SerializeField]
    protected MovementMode currentMode;
    public MovementMode CurrentMode { get { return currentMode; } }

    public void ActivateNavigation(bool _state)
    {
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

    /// <summary>
    /// Used to set the enemy destination
    /// </summary>
    /// <param name="_destination"> The destination for the navMeshAgent.</param>
    public void NavigateTo(Vector3 _destination)
    {
        if (navigation == null) { return; }

        if (navigation.isOnNavMesh)
            navigation.SetDestination(_destination);

        return;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))
            ActivateNavigation(true);

        return;
    }

    public void SetNavigationValues(float _acceleration, float _speed, MovementMode _mode)
    {
        if (navigation == null) { return; }

        currentMode = _mode;
        navigation.acceleration = _acceleration;
        navigation.speed = _speed;

        return;
    }
}
