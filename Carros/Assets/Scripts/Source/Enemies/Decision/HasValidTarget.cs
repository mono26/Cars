using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Decision/HasValidTarget")]
public class HasValidTarget : AIDecision
{
    public override bool Decide(Entity _entity)
    {
        bool decision = false;
        decision = ValidTarget(_entity);

        return decision;
    }

    protected bool ValidTarget(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if (enemy == null) { return false; }

        SlotTargetter slotTargetter = enemy.Targetter as SlotTargetter;
        if (slotTargetter != null && slotTargetter.CurrentSlotTarget != null && slotTargetter.CurrentSlot != null) { return true; }

        Targetter targetter = enemy.Targetter;
        if (targetter != null && targetter.CurrentTarget != null) { return true; }

        return false;
    }
}
