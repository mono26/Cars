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

public class Targetter : AIEntityComponent
{
    [Header("Targetter settings")]
    [SerializeField]
    protected float detectionRange = 5.0f;
    [SerializeField]
    protected float detectionRate = 2.0f; // Detection rate per second
    [SerializeField]
    protected Collider targetterTrigger;
    [SerializeField]
    protected string[] targetableTags;

    [Header("Editor debugging")]
    [SerializeField]
    protected SlotManager currentTarget;
    public SlotManager CurrentTarget { get { return currentTarget; } }
    [SerializeField]
    protected SlotManager.Slot currentSlot = null;
    public SlotManager.Slot CurrentSlot { get { return currentSlot; } }
    [SerializeField]
    protected List<SlotManager> nearTargets;
    [SerializeField]
    protected Coroutine updateRoutine;

    public Vector3 CalculateRandomPointInsideTrigger()
    {
        float randomRadius = Random.Range(0, (int)detectionRange);
        float randomAngle = Random.Range(0, 360);
        float x = randomRadius * Mathf.Cos(randomAngle);
        float z = randomRadius * Mathf.Sin(randomAngle);

        return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
    }

    public override void EveryFrame()
    {
        base.EveryFrame();

        // TODO encapsulate in a routine.
        currentTarget = GetNearestTarget();

        return;
    }

    protected SlotManager GetNearestTarget()
    {
        if(nearTargets.Count.Equals(0)) { return null; }

        SlotManager nearestTarget = null;
        float distance1 = 0;
        for(int i = 0; i < nearTargets.Count; i++)
        {
            SlotManager target = nearTargets[i];
            // TODO check if is dead
            if (!target || !target.gameObject.activeInHierarchy)
            {
                nearTargets.RemoveAt(i);
                continue;
            }
            float distance2 = Vector3.Distance(aiEntity.transform.position, nearTargets[i].transform.position);
            if(distance2 < distance1)
            {
                distance1 = distance2;
                nearestTarget = target;
            }
        }
        return nearestTarget;
    }

    protected SlotManager IsAValidSlot(GameObject _object)
    {
        SlotManager isSlot = _object.GetComponent<SlotManager>();
        if (!isSlot) { return null; }
        else { return isSlot; }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        return;
    }

    protected void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (other.CompareTag(tag))
            {
                Debug.Log(other.gameObject + "entered range");

                SlotManager isSlot = IsAValidSlot(other.gameObject);
                if (!isSlot || nearTargets.Contains(isSlot)) { return; }

                if (nearTargets.Count.Equals(0)) { updateRoutine = StartCoroutine(UpdateTarget()); }
                if (!currentTarget)
                {
                    currentTarget = isSlot;
                    currentSlot = currentTarget.Reserve(aiEntity.gameObject);
                }

                nearTargets.Add(isSlot);
            }
        }

        return;
    }

    protected void OnTriggerExit(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (other.CompareTag(tag))
            {
                SlotManager isSlot = IsAValidSlot(other.gameObject);
                if (!isSlot || !nearTargets.Contains(isSlot)) { return; }

                nearTargets.Remove(isSlot);

                if (currentTarget.Equals(isSlot))
                {
                    currentTarget.Release(currentSlot);
                    currentSlot = null;
                    SlotManager newTarget = GetNearestTarget();
                    currentTarget = newTarget;
                    if(newTarget != null)
                        currentSlot = newTarget.Reserve(aiEntity.gameObject);
                }

                if (currentSlot == null && currentTarget == null && nearTargets.Count.Equals(0))
                {
                    EventManager.TriggerEvent<TargetterEvent>(new TargetterEvent(aiEntity, TargetterEventType.TargetLost));
                    StopCoroutine(updateRoutine);
                }
            }
        }

        return;
    }

    protected virtual void Start()
    {
        SphereCollider sphere = targetterTrigger as SphereCollider;
        if (sphere)
            sphere.radius = detectionRange;

        return;
    }

    protected IEnumerator UpdateTarget()
    {
        SlotManager nearestTarget = GetNearestTarget();

        if(!nearestTarget) { yield return new WaitForSeconds(1 / detectionRate); ; }

        else
        {
            if (!currentTarget.Equals(nearestTarget)) { currentTarget.Release(currentSlot); }

            currentTarget = nearestTarget;
            currentSlot = nearestTarget.Reserve(aiEntity.gameObject);

            yield return new WaitForSeconds(1 / detectionRate);
        }

        updateRoutine = StartCoroutine(UpdateTarget());
    }
}
