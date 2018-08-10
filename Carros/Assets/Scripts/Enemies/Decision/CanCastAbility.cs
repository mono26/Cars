﻿using UnityEngine;

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

        if(enemy.Targetter == null || enemy.Targetter.CurrentTarget == null || enemy.Targetter.CurrentSlot == null) { return false; }

        if(!enemy.Targetter.CurrentSlot.type.Equals(SlotManager.Slot.Type.Waiting))
        {
            foreach (Ability ability in enemy.Abilities)
            {
                if (ability.IsInRange(enemy.Targetter.CurrentTarget.transform) == true && ability.IsInCooldown() == false)
                {
                    Debug.Log(enemy.gameObject.name + " Can cast" + ability.ToString());
                    enemy.SetNextAbility(ability);
                    return true;
                }
            }
        }

        return false;
    }
}
