// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotTargetetterTarget
{
    [Header("Slot Targetter Target editor debugging")]
    [SerializeField] private SlotManager currentSlotManagerTarget;
    [SerializeField] private Slot currentSlotManagerTargetSlot;

    public SlotManager GetCurrentSlotManagerTarget { get { return currentSlotManagerTarget; } }
    public Slot GetCurrentSlotManagerTargetSlot { get { return currentSlotManagerTargetSlot; } }

    public SlotTargetetterTarget(SlotManager _currentSlotManagerTarget = null, Slot _currentSlotManagerTargetSlot = null)
    {
        currentSlotManagerTarget = _currentSlotManagerTarget;
        currentSlotManagerTargetSlot = _currentSlotManagerTargetSlot;
        return;
    }

    public void ReleaseSlotFromTargetSlotManager()
    {
        if (HasValisTargetAndSlot()) {
            currentSlotManagerTarget.Release(currentSlotManagerTargetSlot);
        }
        return;
    }

    public bool HasValisTargetAndSlot()
    {
        bool hasValidTargetAndslot = true;
        if(currentSlotManagerTarget == null || currentSlotManagerTargetSlot == null) {
            hasValidTargetAndslot = false;
        }
        return hasValidTargetAndslot;
    }
}

public class SlotTargetter : Targetter
{
    [Header("SlotTargetter editor debugging")]
    [SerializeField] private SlotTargetetterTarget currentSlotTarget;
    [SerializeField] private List<SlotManager> nearSlotManagerTargets = new List<SlotManager>();

    private Coroutine updateSlotTargetRoutine;

    public SlotTargetetterTarget GetCurrentSlotManagerTarget { get { return currentSlotTarget; } }

    protected override void OnTriggerEnter(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (other.CompareTag(tag))
            {
                SlotManager possibleSlotManagerToAdd = HasASlotManagerComponent(other.gameObject);
                if (possibleSlotManagerToAdd != null && !nearSlotManagerTargets.Contains(possibleSlotManagerToAdd)) {
                    AddSlotedTargetAndStartUpdateProcess(possibleSlotManagerToAdd);
                }
                // If there is no posible SlotManager we execute our parent.
                else if (possibleSlotManagerToAdd == null) {
                    base.OnTriggerEnter(other);
                }
            }
        }
        return;
    }

    private void AddSlotedTargetAndStartUpdateProcess(SlotManager _slotManagerTarget)
    {
        if (currentSlotTarget.GetCurrentSlotManagerTarget == null)
        {
            Slot targetSlot = _slotManagerTarget.Reserve(entity.gameObject);
            currentSlotTarget = new SlotTargetetterTarget(_slotManagerTarget, targetSlot);
        }
        if (nearSlotManagerTargets.Count.Equals(0)) {
            updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget());
        }
        nearSlotManagerTargets.Add(_slotManagerTarget);
        return;
    }

    private IEnumerator UpdateSlotTarget()
    {
        SlotTargetetterTarget newSlotManagerTarget = GetNearestActiveSlotManagerTargetAndSlot();
        if (!currentSlotTarget.Equals(newSlotManagerTarget))
        {
            currentSlotTarget.ReleaseSlotFromTargetSlotManager();
            currentSlotTarget = newSlotManagerTarget;
        }
        yield return new WaitForSeconds(1 / detectionRate);
        updateSlotTargetRoutine = StartCoroutine(UpdateSlotTarget());
        yield break;
    }

    private SlotTargetetterTarget GetNearestActiveSlotManagerTargetAndSlot()
    {
        SlotManager nearestTarget = null;
        Slot nearestSlot = null;
        if (nearSlotManagerTargets.Count > 0)
        {
            nearSlotManagerTargets = HelperMethods.ClearInactiveComponentsInCollection(nearSlotManagerTargets);
            nearestTarget = HelperMethods.GetComponentAtMinimumDistanceInColection(nearSlotManagerTargets, entity.transform);
            nearestSlot = nearestTarget.Reserve(entity.gameObject);
        }
        SlotTargetetterTarget targetToReturn = new SlotTargetetterTarget(nearestTarget, nearestSlot);
        return targetToReturn;
    }

    protected override void OnTriggerExit(Collider other)
    {
        foreach (string tag in targetableTags)
        {
            if (other.CompareTag(tag))
            {
                SlotManager possibleSlotManagerToRemove = HasASlotManagerComponent(other.gameObject);
                if (possibleSlotManagerToRemove != null && nearSlotManagerTargets.Contains(possibleSlotManagerToRemove)) {
                    RemoveSlotedTarget(possibleSlotManagerToRemove);
                }
                else if (possibleSlotManagerToRemove == null) {
                    base.OnTriggerExit(other);
                }
            }
        }
        return;
    }

    private void RemoveSlotedTarget(SlotManager _slotedTarget)
    {
        CheckIfRemovedIsCurrentSlotManagerTargetAndAssignNew(_slotedTarget);
        nearSlotManagerTargets.Remove(_slotedTarget);
        if (NoSlotManagerTargetsNear())
        {
            EventManager.TriggerEvent<TargetterEvent>(new TargetterEvent(entity as Enemy, TargetterEventType.TargetLost));
            StopCoroutine(updateSlotTargetRoutine);
        }
        return;
    }

    private void CheckIfRemovedIsCurrentSlotManagerTargetAndAssignNew(SlotManager _slotedTarget)
    {
        if (currentSlotTarget.GetCurrentSlotManagerTarget.Equals(_slotedTarget))
        {
            currentSlotTarget.ReleaseSlotFromTargetSlotManager();
            SlotTargetetterTarget newNearestSlotManagerTarget = GetNearestActiveSlotManagerTargetAndSlot();
            currentSlotTarget = newNearestSlotManagerTarget;
        }
        return;
    }

    private bool NoSlotManagerTargetsNear()
    {
        return currentSlotTarget.GetCurrentSlotManagerTarget == null && nearSlotManagerTargets.Count.Equals(0);
    }

    private SlotManager HasASlotManagerComponent(GameObject _object)
    {
        SlotManager isSlot = _object.GetComponent<SlotManager>();
        if (isSlot == null)
        {
            isSlot = _object.GetComponentInParent<SlotManager>();
        }
        return isSlot;
    }

    public override Vector3 GetCurrentTargetPosition()
    {
        // Default value if there is no target.
        Vector3 targetPosition = entity.transform.position;
        if(currentSlotTarget.GetCurrentSlotManagerTarget != null) {
            targetPosition = currentSlotTarget.GetCurrentSlotManagerTarget.GetSlotPosition(
                currentSlotTarget.GetCurrentSlotManagerTargetSlot
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
        if(currentSlotTarget.GetCurrentSlotManagerTargetSlot != null)
        {
            currentType = currentSlotTarget.GetCurrentSlotManagerTargetSlot.GetSlotType;
        }
        return currentType;
    }

    public override bool HasAValidCurrentTarget()
    {
        bool hasAValidCurrentTarget = false;
        //HelperMethods.DebugMessageWithObjectAndTimeStamp("Has current target", currentSlotTarget.GetCurrentSlotManagerTarget);
        if (currentSlotTarget.HasValisTargetAndSlot()) {
            hasAValidCurrentTarget = true;
        }
        else {
            hasAValidCurrentTarget = base.HasAValidCurrentTarget();
        }
        return hasAValidCurrentTarget;
    }
}
    