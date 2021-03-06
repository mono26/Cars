﻿// Copyright (c) What a Box Creative Studio. All rights reserved.

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
        bool canCast = false;
        if (_entity is Enemy)
        {
            Enemy enemy = _entity as Enemy;
            if(enemy.CanAttackTarget())
            {
                foreach (Ability ability in enemy.GetAbilitiesComponent)
                {
                    if (ability.IsInRange(enemy.GetTargetPosition()) && !ability.IsInCooldown())
                    {
                        enemy.SetNextAbility(ability);
                        canCast = true;
                    }
                }
            }
        }
        return canCast;
    }
}
