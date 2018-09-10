using System;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/State")]
public class AIState : ScriptableObject
{
    [Header("AI State settings")]
    [SerializeField] private string stateName;
    [SerializeField] private AIAction[] actions;
    [SerializeField] private AITransition[] transitions;

    public string GetStateName { get { return stateName; } }

    public void UpdateState(Entity _entity)
    {
        DoActions(_entity);
        CheckTransitions(_entity);
    }

    private void DoActions(Entity _entity)
    {
        try
        {
            if (StateHasActions())
            {
                for (int i = 0; i < actions.Length; i++)
                    actions[i].DoAction(_entity);
            }
        }
        catch (AIStateException stateException)
        {
            stateException.NoActionException();
        }
        return;
    }

    private bool StateHasActions()
    {
        bool hasActions = true;
        if (actions.Length == 0)
        {
            hasActions = false;
            throw new AIStateException(this, AIStateException.AIStateExceptionType.NoAction);
        }
        return hasActions;
    }

    private void CheckTransitions(Entity _entity)
    {
        try
        {
            if (StateHasTransition() && _entity is Enemy)
            {
                Enemy enemy = _entity as Enemy;
                for (int i = 0; i < transitions.Length; i++)
                {
                    bool decisionState = transitions[i].GetDecision.Decide(_entity);
                    if (decisionState == true) {
                        enemy.ChangeState(transitions[i].GetTrueState);
                    }
                    else {
                        enemy.ChangeState(transitions[i].GetFalseState);
                    }
                }
            }
        }
        catch (AIStateException stateException)
        {
            stateException.LaunchException();
        }
        return;
    }

    private bool StateHasTransition()
    {
        bool hasTransitions = true;
        if (transitions.Length == 0)
        {
            hasTransitions = false;
            throw new AIStateException(this, AIStateException.AIStateExceptionType.NoTransition);
        }
        return hasTransitions;
    }
}


