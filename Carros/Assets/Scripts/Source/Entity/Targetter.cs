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
    [SerializeField]
    protected float detectionRange = 5.0f;
    [SerializeField]
    protected float detectionRate = 2.0f; // Detection rate per second
    [SerializeField]
    protected SphereCollider detectionTrigger;
    [SerializeField]
    protected string[] targetableTags;

    [Header("Editor debugging")]
    [SerializeField]
    protected Transform currentTarget;
    public Transform CurrentTarget { get { return currentTarget; } }
    [SerializeField]
    protected List<Transform> nearTargets;
    [SerializeField]
    protected Coroutine updateTargetRoutine;

    protected override void Awake()
    {
        if (detectionTrigger == null)
            detectionTrigger = GetComponent<SphereCollider>();
        if (detectionTrigger == null)
            throw new MissingComponentException("There is no component attached to the object: ", typeof(SphereCollider));
    }

    protected virtual void Start()
    {
        if (HasDetectionTrigger())
            detectionTrigger.radius = detectionRange;
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

    protected Transform GetNearestActiveTarget()
    {
        if(nearTargets.Count.Equals(0)) { return null; }
        HelperMethods.ClearInactiveElementsInCollection(ref nearTargets);
        Transform nearestTarget = HelperMethods.GetElementAtMinimumDistanceInColection(nearTargets, entity.transform);
        return nearestTarget;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        return;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag)) { return; }
            Transform posibleTarget = other.transform;
            if (posibleTarget != null && !nearTargets.Contains(posibleTarget))
                AddTarget(posibleTarget);
        }
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

    protected virtual void OnTriggerExit(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag)) { return; }
            Transform realisingTarget = other.transform;
            if (realisingTarget  == null || !nearTargets.Contains(realisingTarget)) { return; }
            RemoveTarget(realisingTarget);
        }
        return;
    }

    protected void RemoveTarget(Transform _target)
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
}
