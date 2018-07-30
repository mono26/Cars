using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/Decision/CanCastAbility")]
public class CanCastAbility : Decision
{
    public override bool Decide(Entity _entity)
    {
        bool decision = false;
        return decision; 
    }

    protected void CanCast(Entity _entity)
    {
        Enemy enemy = _entity as Enemy;
        if(enemy == null) { return; }

        foreach (Ability ability in enemy.Abilities)
        {
            if (ability.IsInRange() == true)
            {
                enemy.SetNextAbility(ability);
                break;
            }
            // Is not in Range do nothing
        }

        return;
    }
}
