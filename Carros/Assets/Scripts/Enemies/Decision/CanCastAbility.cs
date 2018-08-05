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

        foreach (Ability ability in enemy.Abilities)
        {
            if (ability.IsInRange() == true && ability.IsInCooldown() == false)
            {
                Debug.Log(enemy.gameObject.name + "Can cast" + ability.ToString());
                enemy.SetNextAbility(ability);
                return true;
            }
        }

        return false;
    }
}
