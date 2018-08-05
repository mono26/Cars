using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected int currentTargetSlot;
    public int CurrentTargetSlot { get { return currentTargetSlot; } }
    [SerializeField]
    protected List<SlotManager> nearTargets;
    [SerializeField]
    protected Coroutine updateRoutine;

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        return;
    }

    protected virtual void Start()
    {
        SphereCollider sphere = targetterTrigger as SphereCollider;
        if (sphere)
            sphere.radius = detectionRange;

        updateRoutine = StartCoroutine(UpdateTarget());

        return;
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
        if(nearTargets.Count == 0) { return null; }

        SlotManager nearestTarget = null;
        float distance1 = 0;
        for(int i = 0; i < nearTargets.Count; i++)
        {
            // TODO check if is dead
            if (!nearTargets[i]|| !nearTargets[i].gameObject.activeInHierarchy)
            {
                nearTargets.RemoveAt(i);
                continue;
            }
            float distance2 = Vector3.Distance(aiEntity.transform.position, nearTargets[i].transform.position);
            if(distance2 < distance1)
            {
                distance1 = distance2;
                nearestTarget = nearTargets[i];
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

    protected void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (other.CompareTag(tag))
            {
                Debug.Log(other.gameObject + "entered range");

                SlotManager isSlot = IsAValidSlot(other.gameObject);
                if (!isSlot || nearTargets.Contains(isSlot)) { return; }

                nearTargets.Add(isSlot);
                if (currentTarget == null) { currentTarget = isSlot; }
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
                if (isSlot || !nearTargets.Contains(isSlot)) { return; }

                nearTargets.Remove(isSlot);
                if (currentTarget.Equals(isSlot)) { currentTarget = null; }
                // Assign new target if possible.
            }
        }

        return;
    }

    protected IEnumerator UpdateTarget()
    {
        currentTarget = GetNearestTarget();

        if(!currentTarget) { yield return new WaitForSeconds(1 / detectionRate); ; }

        else
        {
            currentTargetSlot = currentTarget.Reserve(gameObject);

            yield return new WaitForSeconds(1 / detectionRate);
        }

        updateRoutine = StartCoroutine(UpdateTarget());
    }
}
