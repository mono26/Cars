using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Decision/CanCastAbility")]
public class CanCastAbility : AIDecision
{
    public override bool Decide(Entity _entity)
    {
        bool decision = false;
        decision = CanCast(_entity);

        return decision; 
    }

    protected bool CanCast(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if(enemy == null) { return false; }

        SlotTargetter slotTargetter = enemy.Targetter as SlotTargetter;
        if (slotTargetter != null && slotTargetter.CurrentSlotTarget != null && slotTargetter.CurrentSlot != null)
        {
            if (!slotTargetter.CurrentSlot.type.Equals(SlotManager.Slot.Type.Waiting))
            {
                foreach (Ability ability in enemy.Abilities)
                {
                    if (ability.IsInRange(slotTargetter.CurrentTarget) && !ability.IsInCooldown())
                    {
                        Debug.Log(enemy.gameObject.name + " Can cast" + ability.ToString());
                        enemy.SetNextAbility(ability);
                        return true;
                    }
                }
            }
            return false;
        }

        Targetter targetter = enemy.Targetter;
        if(targetter != null && targetter.CurrentTarget != null)
        {
            foreach (Ability ability in enemy.Abilities)
            {
                if (ability.IsInRange(slotTargetter.CurrentTarget) && !ability.IsInCooldown())
                {
                    Debug.Log(enemy.gameObject.name + " Can cast" + ability.ToString());
                    enemy.SetNextAbility(ability);
                    return true;
                }
            }
            return false;
        }

        return false;
    }
}
