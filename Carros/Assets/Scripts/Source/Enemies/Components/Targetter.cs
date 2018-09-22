using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetterEventType { TargetLost, TargetAcquired }

public class TargetterEvent : CarEvent
{
    public Enemy enemy;
    public TargetterEventType eventType;

    public TargetterEvent(Enemy _enemy, TargetterEventType _eventType)
    {
        enemy = _enemy;
        eventType = _eventType;
    }
}

public class Targetter : EntityComponent
{
    [Header("Targetter settings")]
    [SerializeField] private float detectionRange = 5.0f;
    [SerializeField] protected float detectionRate = 2.0f; // Detection rate per second
    [SerializeField] protected string[] targetableTags = new string[] { "Player" };

    [Header("Targetter components")]
    [SerializeField] private SphereCollider detectionTrigger;

    [Header("Targetter editor debugging")]
    [SerializeField] private Transform currentTarget;
    [SerializeField] private List<Transform> nearTargets;

    private Coroutine updateTargetRoutine;

    public Transform GetCurrentTarget { get { return currentTarget; } }

    protected override void Awake()
    {
        if (entity == null)
        {
            entity = GetComponent<Entity>();
            if(entity == null) {
                entity = GetComponentInParent<Entity>();
            }
        }
        if (detectionTrigger == null) {
            detectionTrigger = GetComponent<SphereCollider>();
        }
        return;
    }

    protected virtual void Start()
    {
        try
        {
            if (HasDetectionTrigger())
                detectionTrigger.radius = detectionRange;
        }
        catch(MissingComponentException missingComponentExecption) {
            missingComponentExecption.DisplayException();
        }
        return;
    }

    protected bool HasDetectionTrigger()
    {
        bool hasTrigger = true;
        if (detectionTrigger == null)
        {
            hasTrigger = false;
            throw new MissingComponentException("The enemy has a missing component: ", typeof(SphereCollider));
        }
        return hasTrigger;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (other.CompareTag(tag))
            {
                Transform posibleTarget = other.transform;
                if (CanAddTarget(posibleTarget)) {
                    AddTarget(posibleTarget);
                }
            }
        }
        return;
    }

    private bool CanAddTarget(Transform _targetToAdd)
    {
        bool canAdd = true;
        if (_targetToAdd == null && nearTargets.Contains(_targetToAdd)) {
            canAdd = false;
        }
        return canAdd;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (other.CompareTag(tag))
            {
                Transform targetToRemove = other.transform;
                if (CanRemoveTarget(targetToRemove)) {
                    RemoveTarget(targetToRemove);
                }
            }
        }
        return;
    }

    private bool CanRemoveTarget(Transform _targetToRelease)
    {
        bool canRelease = true;
        if (_targetToRelease == null || !nearTargets.Contains(_targetToRelease)) {
            canRelease = false;
        }
        return canRelease;
    }

    private Transform GetNearestActiveTarget()
    {
        if(nearTargets.Count.Equals(0)) { return null; }
        HelperMethods.ClearInactiveElementsInCollection(ref nearTargets);
        Transform nearestTarget = HelperMethods.GetElementAtMinimumDistanceInColection(nearTargets, entity.transform);
        return nearestTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        return;
    }

    private void AddTarget(Transform _target)
    {
        if (nearTargets.Count.Equals(0)) { updateTargetRoutine = StartCoroutine(UpdateTarget()); }
        if (currentTarget == null) { currentTarget = _target; }
        nearTargets.Add(_target);
        return;
    }

    private IEnumerator UpdateTarget()
    {
        Transform nearestTarget = GetNearestActiveTarget();
        if (!nearestTarget) { yield return new WaitForSeconds(1 / detectionRate); ; }
        else
        {
            currentTarget = nearestTarget;
            yield return new WaitForSeconds(1 / detectionRate);
        }
        updateTargetRoutine = StartCoroutine(UpdateTarget());
    }

    private void RemoveTarget(Transform _target)
    {
        nearTargets.Remove(_target);
        if (currentTarget.Equals(_target))
        {
            Transform newTarget = GetNearestActiveTarget();
            currentTarget = newTarget;
        }
        if (currentTarget == null && nearTargets.Count.Equals(0))
        {
            EventManager.TriggerEvent<TargetterEvent>(new TargetterEvent(entity as Enemy, TargetterEventType.TargetLost));
            StopCoroutine(updateTargetRoutine);
        }
        return;
    }

    public Vector3 CalculateRandomPointInsideTrigger()
    {
        int randomRadius = Random.Range(0, (int)detectionRange);
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float x = randomRadius * Mathf.Cos(randomAngle);
        float z = randomRadius * Mathf.Sin(randomAngle);
        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }

    public virtual Vector3 GetCurrentTargetPosition()
    {
        // Default value if there is no target.
        Vector3 targetPosition = entity.transform.position;
        if (currentTarget != null) {
            targetPosition = currentTarget.position;
        }
        return targetPosition;
    }

    public virtual bool HasAValidCurrentTarget()
    {
        bool hasAValidCurrentTarget = true;
        if(currentTarget == null) {
            hasAValidCurrentTarget = false;
        }
        return hasAValidCurrentTarget;
    }
}
