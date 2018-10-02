using UnityEngine;
using UnityEngine.AI;

public enum EnemyMovementMode { Patrolling, Running }
public enum EnemyMovemenState { Idle, Running, Walking}

public class EnemyMovementEvent : GameEvent
{
    public Enemy enemy;
    public EnemyMovementMode movementType;

    public EnemyMovementEvent(Enemy _enemy, EnemyMovementMode _movementType)
    {
        enemy = _enemy;
        movementType = _movementType;
        return;
    }
}

[System.Serializable]
public class MovementStats
{
    [SerializeField] private float runningAcceleration;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float walkingAcceleration;
    [SerializeField] private float walkingSpeed;

    public float GetRunningAcceleration { get { return runningAcceleration; } }
    public float GetRunningSpeed { get { return runningSpeed; } }
    public float GetWalkingAcceleration { get { return walkingAcceleration; } }
    public float GetWalkingSpeed { get { return walkingSpeed; } }

    public MovementStats(int _runningAcceleration, int _runningSpeed, int _walkingAcceleration, int _walkingSpeed)
    {
        runningAcceleration = _runningAcceleration;
        runningSpeed = _runningSpeed;
        walkingAcceleration = _walkingAcceleration;
        walkingSpeed = _walkingSpeed;
        return;
    }
}

public class EnemyMovement : EntityComponent
{
    [Header("Enemy Movement settings")]
    [SerializeField] private MovementStats movementStats;

    [Header("Enemy Movement Components")]
    [SerializeField] private NavMeshAgent navigationComponent;

    public MovementStats GetMovementStats { get { return movementStats; } }
    public NavMeshAgent GetNavigationComponent { get { return navigationComponent; } }
    public float GetNavigationSpeed { get { return navigationComponent.velocity.magnitude; } }

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
        if(HasNavigationComponent()) {
            navigationComponent.updatePosition = false;
        }
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

    private bool CheckIfEnemyIsStandingStill()
    {
        bool isStandingStill = true;
        try
        {
            if (HasNavigationComponent()) {
                isStandingStill = navigationComponent.desiredVelocity.sqrMagnitude <= 0.1f * 0.1f ? true : false;
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
            throw new MissingComponentException(gameObject, typeof(NavMeshAgent));
        }
        return hasNavigation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Terrain"))
        {
            ActivateNavMeshNavigation(true);
            entity.SetIfBodyAffectedByGravity(false);
        }
        return;
    }

    public void ActivateNavMeshNavigation(bool _state)
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
            }

        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    public void SetNavigationValuesDependingOnMode(EnemyMovementMode _mode)
    {
        if (HasNavigationComponent())
        {
            if(_mode == EnemyMovementMode.Patrolling)
            {
                navigationComponent.acceleration = movementStats.GetWalkingAcceleration;
                navigationComponent.speed = movementStats.GetWalkingSpeed;
            }
            else
            {
                navigationComponent.acceleration = movementStats.GetRunningAcceleration;
                navigationComponent.speed = movementStats.GetRunningSpeed;
            }
        }
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
