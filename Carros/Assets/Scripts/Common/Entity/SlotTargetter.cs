using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTargetter : Targetter
{
    [Header("Editor debugging")]
    [SerializeField]
    protected SlotManager currentSlotTarget;
    public SlotManager CurrentSlotTarget { get { return currentSlotTarget; } }
    [SerializeField]
    protected SlotManager.Slot currentSlot = null;
    public SlotManager.Slot CurrentSlot { get { return currentSlot; } }
    [SerializeField]
    protected List<SlotManager> nearSlotTargets;
    [SerializeField]
    protected Coroutine updateSlotTargetRoutine;

    protected void AddSlotedTarget(SlotManager _slotedTarget)
    {
        if (nearSlotTargets.Count.Equals(0)) { updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget()); }
        if (currentSlotTarget == null)
        {
            currentSlotTarget = _slotedTarget;
            currentSlot = currentSlotTarget.Reserve(aiEntity.gameObject);
        }

        nearSlotTargets.Add(_slotedTarget);

        return;
    }

    protected SlotManager GetNearestSlotTarget()
    {
        if (nearTargets.Count.Equals(0)) { return null; }

        SlotManager nearestTarget = null;
        float distance1 = 0;
        for (int i = 0; i < nearTargets.Count; i++)
        {
            SlotManager target = nearSlotTargets[i];
            // TODO check if is dead
            if (!target || !target.gameObject.activeInHierarchy)
            {
                nearTargets.RemoveAt(i);
                continue;
            }
            float distance2 = Vector3.Distance(aiEntity.transform.position, nearTargets[i].transform.position);
            if (distance2 < distance1)
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

    protected override void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag)){ return; }

            SlotManager possibleSlot = IsAValidSlot(other.gameObject);
            Debug.Log("Entered range: " + possibleSlot);
            if (possibleSlot != null|| !nearSlotTargets.Contains(possibleSlot))
                AddSlotedTarget(possibleSlot);

            else base.OnTriggerEnter(other);
        }
        return;
    }

    protected override void OnTriggerExit(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag)) { return; }

            SlotManager possibleSlot = IsAValidSlot(other.gameObject);
            if (possibleSlot || nearSlotTargets.Contains(possibleSlot))
                RemoveSlotedTarget(possibleSlot);

            else base.OnTriggerExit(other);
        }
        return;
    }

    protected void RemoveSlotedTarget(SlotManager _slotedTarget)
    {
        nearSlotTargets.Remove(_slotedTarget);

        if (currentSlotTarget.Equals(_slotedTarget))
        {
            currentSlotTarget.Release(currentSlot);
            currentSlot = null;
            SlotManager newTarget = GetNearestSlotTarget();
            currentSlotTarget = newTarget;
            if (newTarget != null)
                currentSlot = newTarget.Reserve(aiEntity.gameObject);
        }

        if (currentSlot == null && currentSlotTarget == null && nearTargets.Count.Equals(0))
        {
            EventManager.TriggerEvent<TargetterEvent>(new TargetterEvent(aiEntity, TargetterEventType.TargetLost));
            StopCoroutine(updateSlotTargetRoutine);
        }

        return;
    }

    protected IEnumerator UpdateSlotTarget()
    {
        SlotManager nearestTarget = GetNearestSlotTarget();

        if (!nearestTarget) { yield return new WaitForSeconds(1 / detectionRate); ; }

        else
        {
            if (!currentTarget.Equals(nearestTarget)) { currentSlotTarget.Release(currentSlot); }

            currentSlotTarget = nearestTarget;
            currentSlot = nearestTarget.Reserve(aiEntity.gameObject);

            yield return new WaitForSeconds(1 / detectionRate);
        }

        updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget());
    }
}
