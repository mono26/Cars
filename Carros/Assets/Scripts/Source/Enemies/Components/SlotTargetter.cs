using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManagerTarget
{
    private SlotManager currentSlotManagerTarget;
    private Slot currentSlotManagerTargetSlot;

    public SlotManager GetCurrentSlotManagerTarget { get { return currentSlotManagerTarget; } }
    public Slot GetCurrentSlotManagerTargetSlot { get { return currentSlotManagerTargetSlot; } }

    public SlotManagerTarget(SlotManager _currentSlotManagerTarget, Slot _currentSlotManagerTargetSlot)
    {
        currentSlotManagerTarget = _currentSlotManagerTarget;
        currentSlotManagerTargetSlot = _currentSlotManagerTargetSlot;
        return;
    }
}

public class SlotTargetter : Targetter
{
    [Header("SlotTargetter editor debugging")]
    [SerializeField] private SlotManagerTarget currentSlotManagerTarget;
    [SerializeField] private List<SlotManager> nearSlotManagerTargets = new List<SlotManager>();

    private Coroutine updateSlotTargetRoutine;

    public SlotManagerTarget GetCurrentSlotManagerTarget { get { return currentSlotManagerTarget; } }

    protected override void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag))
            {
                SlotManager possibleSlotManagerTarget = HasASlotManagerComponent(other.gameObject);
                if (possibleSlotManagerTarget != null && !nearSlotManagerTargets.Contains(possibleSlotManagerTarget)) {
                    AddSlotedTarget(possibleSlotManagerTarget);
                }
                // If there is no posible SlotManager we execute our parent.
                else if (possibleSlotManagerTarget == null) {
                    base.OnTriggerEnter(other);
                }
            }
        }
        return;
    }

    protected override void OnTriggerExit(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (!other.CompareTag(tag)) { return; }
            SlotManager possibleSlot = HasASlotManagerComponent(other.gameObject);
            if (possibleSlot != null && nearSlotManagerTargets.Contains(possibleSlot)) {
                RemoveSlotedTarget(possibleSlot);
            }
            else { base.OnTriggerExit(other); }
        }
        return;
    }

    private void AddSlotedTarget(SlotManager _slotManagerTarget)
    {
        if (nearSlotManagerTargets.Count.Equals(0)) {
            updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget());
        }
        if (currentSlotManagerTarget == null)
        {
            Slot targetSlot = _slotManagerTarget.Reserve(entity.gameObject);
            currentSlotManagerTarget = new SlotManagerTarget(_slotManagerTarget, targetSlot);
        }
        nearSlotManagerTargets.Add(_slotManagerTarget);
        return;
    }

    private IEnumerator UpdateSlotTarget()
    {
        SlotManager nearestTarget = GetNearestActiveSlotManagerTarget();
        if (nearestTarget == null) {
            yield return new WaitForSeconds(1 / detectionRate);
        }
        else
        {
            SlotManager currentTarget = currentSlotManagerTarget.GetCurrentSlotManagerTarget;
            if (!currentTarget.Equals(nearestTarget))
            {
                currentTarget.Release(currentSlotManagerTarget.GetCurrentSlotManagerTargetSlot);
                Slot nearestTargetSlot = nearestTarget.Reserve(entity.gameObject);
                currentSlotManagerTarget = new SlotManagerTarget(nearestTarget, nearestTargetSlot);
            }
            yield return new WaitForSeconds(1 / detectionRate);
        }
        updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget());
        yield break;
    }

    private SlotManager GetNearestActiveSlotManagerTarget()
    {
        if (nearSlotManagerTargets.Count.Equals(0)) { return null; }
        HelperMethods.ClearInactiveElementsInCollection(ref nearSlotManagerTargets);
        SlotManager nearestTarget = HelperMethods.GetElementAtMinimumDistanceInColection(nearSlotManagerTargets, entity.transform);
        return nearestTarget;
    }

    private SlotManager HasASlotManagerComponent(GameObject _object)
    {
        SlotManager isSlot = _object.GetComponent<SlotManager>();
        if (isSlot == null) {
            isSlot = _object.GetComponentInParent<SlotManager>();
        }
        return isSlot;
    }

    private void RemoveSlotedTarget(SlotManager _slotedTarget)
    {
        nearSlotManagerTargets.Remove(_slotedTarget);
        SlotManager currentTarget = currentSlotManagerTarget.GetCurrentSlotManagerTarget;
        if (currentTarget.Equals(_slotedTarget))
        {
            currentTarget.Release(currentSlotManagerTarget.GetCurrentSlotManagerTargetSlot);
            SlotManager newTarget = GetNearestActiveSlotManagerTarget();
            currentSlotManagerTarget = null;
            if (newTarget != null)
            {
                Slot newSlot = newTarget.Reserve(entity.gameObject);
                currentSlotManagerTarget = new SlotManagerTarget(newTarget, newSlot);
            }
        }
        if (currentSlotManagerTarget == null && nearSlotManagerTargets.Count.Equals(0))
        {
            EventManager.TriggerEvent<TargetterEvent>(new TargetterEvent(entity as Enemy, TargetterEventType.TargetLost));
            StopCoroutine(updateSlotTargetRoutine);
        }
        return;
    }

    public override Vector3 GetCurrentTargetPosition()
    {
        // Default value if there is no target.
        Vector3 targetPosition = entity.transform.position;
        if(currentSlotManagerTarget != null) {
            targetPosition = currentSlotManagerTarget.GetCurrentSlotManagerTarget.GetSlotPosition(
                currentSlotManagerTarget.GetCurrentSlotManagerTargetSlot
                );
        }
        else {
            targetPosition = base.GetCurrentTargetPosition();
        }
        return targetPosition;
    }

    public SlotType GetCurrentSlotType()
    {
        SlotType currentType = SlotType.Waiting;
        if(currentSlotManagerTarget != null)
        {
            currentType = currentSlotManagerTarget.GetCurrentSlotManagerTargetSlot.GetSlotType;
        }
        return currentType;
    }

    public override bool HasAValidCurrentTarget()
    {
        bool hasAValidCurrentTarget = true;
        if (currentSlotManagerTarget == null) {
            hasAValidCurrentTarget = false;
        }
        else {
            hasAValidCurrentTarget = base.HasAValidCurrentTarget();
        }
        return base.HasAValidCurrentTarget();
    }
}
    