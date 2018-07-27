using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/State")]
public class AIState : ScriptableObject
{
    public AIAction[] actions;
    public AITransition[] transitions;

    public void UpdateState(Entity controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(Entity controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].DoAction(controller);
        }
    }

    private void CheckTransitions(Entity _entity)
    {
        if (transitions.Length > 0)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionState = transitions[i].decision.Decide(_entity);
                /*if (decisionState)
                    //_entity._stateHandler.TransitionToState(transitions[i].trueState);
                else
                    //_entity._stateHandler.TransitionToState(transitions[i].falseState);*/
            }
        }

        return;
    }
}
