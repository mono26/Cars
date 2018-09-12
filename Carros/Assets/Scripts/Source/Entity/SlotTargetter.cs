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

    protected override void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag)){ return; }
            SlotManager possibleSlotTarget = IsAValidSlot(other.gameObject);
            if (possibleSlotTarget != null && !nearSlotTargets.Contains(possibleSlotTarget))
                AddSlotedTarget(possibleSlotTarget);
            // If the posibleSlot is already contained we exit.
            else if (possibleSlotTarget && nearSlotTargets.Contains(possibleSlotTarget)) { return; }
            else { base.OnTriggerEnter(other); }
        }
        return;
    }

    private void AddSlotedTarget(SlotManager _slotedTarget)
    {
        if (nearSlotTargets.Count.Equals(0)) { updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget()); }
        if (currentSlotTarget == null)
        {
            currentSlotTarget = _slotedTarget;
            currentSlot = currentSlotTarget.Reserve(entity.gameObject);
        }
        nearSlotTargets.Add(_slotedTarget);
        return;
    }

    private IEnumerator UpdateSlotTarget()
    {
        SlotManager nearestTarget = GetNearestActiveSlotTarget();
        if (nearestTarget == null) { yield return new WaitForSeconds(1 / detectionRate); ; }
        else
        {
            if (!currentTarget.Equals(nearestTarget)) { currentSlotTarget.Release(currentSlot); }
            currentSlotTarget = nearestTarget;
            currentSlot = nearestTarget.Reserve(entity.gameObject);
            yield return new WaitForSeconds(1 / detectionRate);
        }
        updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget());
        yield break;
    }

    private SlotManager GetNearestActiveSlotTarget()
    {
        if (nearTargets.Count.Equals(0)) { return null; }
        HelperMethods.ClearInactiveElementsInCollection(ref nearSlotTargets);
        SlotManager nearestTarget = HelperMethods.GetElementAtMinimumDistanceInColection(nearSlotTargets, entity.transform);
        return nearestTarget;
    }

    private SlotManager IsAValidSlot(GameObject _object)
    {
        SlotManager isSlot = _object.GetComponent<SlotManager>();
        if (isSlot == null)
        {
            isSlot = _object.GetComponentInParent<SlotManager>();
        }
        return isSlot;
    }

    protected override void OnTriggerExit(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag)) { return; }
            SlotManager possibleSlot = IsAValidSlot(other.gameObject);
            if (possibleSlot != null && nearSlotTargets.Contains(possibleSlot))
                RemoveSlotedTarget(possibleSlot);
            else { base.OnTriggerExit(other); }
        }

        return;
    }

    private void RemoveSlotedTarget(SlotManager _slotedTarget)
    {
        nearSlotTargets.Remove(_slotedTarget);
        if (currentSlotTarget.Equals(_slotedTarget))
        {
            currentSlotTarget.Release(currentSlot);
            currentSlot = null;
            SlotManager newTarget = GetNearestActiveSlotTarget();
            currentSlotTarget = newTarget;
            if (newTarget != null)
                currentSlot = newTarget.Reserve(entity.gameObject);
        }
        if (currentSlot == null && currentSlotTarget == null && nearTargets.Count.Equals(0))
        {
            EventManager.TriggerEvent<TargetterEvent>(new TargetterEvent(entity as Enemy, TargetterEventType.TargetLost));
            StopCoroutine(updateSlotTargetRoutine);
        }
        return;
    }
}
    