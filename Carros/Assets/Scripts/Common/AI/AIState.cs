using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/State")]
public class AIState : ScriptableObject
{
    public string stateName;
    public AIAction[] actions;
    public AITransition[] transitions;

    public void UpdateState(Entity _entity)
    {
        DoActions(_entity);
        CheckTransitions(_entity);
    }

    private void DoActions(Entity _entity)
    {
        if (actions.Length == 0) { return; }

        for (int i = 0; i < actions.Length; i++)
            actions[i].DoAction(_entity);

        return;
    }

    private void CheckTransitions(Entity _entity)
    {
        if (transitions.Length == 0) { return; }

        Enemy enemy = _entity as Enemy;
        if(enemy == null) { return; }

        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionState = transitions[i].decision.Decide(_entity);
            if (decisionState == true)
                enemy.StateMachine.ChangeState(transitions[i].trueState);
            else
                enemy.StateMachine.ChangeState(transitions[i].falseState);
        }

        return;
    }
}
