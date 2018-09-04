using System;
using UnityEngine;

[CreateAssetMenu(menuName = "AIComponents/State")]
public class AIState : ScriptableObject
{
    [Header("AI State settings")]
    [SerializeField] protected string stateName;
    [SerializeField] protected AIAction[] actions;
    [SerializeField] protected AITransition[] transitions;

    public string GetStateName { get { return stateName; } }

    public void UpdateState(Entity _entity)
    {
        DoActions(_entity);
        CheckTransitions(_entity);
    }

    protected void DoActions(Entity _entity)
    {
        try
        {
            if (actions.Length == 0) { throw new AIStateException(this); }

            for (int i = 0; i < actions.Length; i++)
                actions[i].DoAction(_entity);
        }
        catch (AIStateException stateException)
        {
            stateException.NoActionException();
        }

        return;
    }

    protected void CheckTransitions(Entity _entity)
    {
        try
        {
            if (transitions.Length == 0) { throw new AIStateException(this, AIStateException.AIStateExceptionType.NoTransition); }

            Enemy enemy = _entity as Enemy;
            if (enemy == null) { throw new AIStateException(this, AIStateException.AIStateExceptionType.NoStateMachine); }

            for (int i = 0; i < transitions.Length; i++)
            {
                bool decisionState = transitions[i].GetDecision.Decide(_entity);
                if (decisionState == true)
                    enemy.StateMachine.ChangeState(transitions[i].GetTrueState);
                else
                    enemy.StateMachine.ChangeState(transitions[i].GetTrueState);
            }
        }
        catch (AIStateException stateException)
        {
            stateException.LaunchException();
        }

        return;
    }
}

public class AIStateException : Exception
{
    public enum AIStateExceptionType { NoAction, NoStateMachine, NoTransition }

    protected AIStateExceptionType exceptionType;
    protected AIState stateWithException;

    public AIStateExceptionType GetExceptionType { get { return exceptionType; } }

    public AIStateException() : base()
    {
        return;
    }

    public AIStateException(AIState _stateWithException) : base()
    {
        stateWithException = _stateWithException;

        return;
    }

    public AIStateException(AIState _stateWithException, AIStateExceptionType _exceptionType) : base()
    {
        exceptionType = _exceptionType;
        stateWithException = _stateWithException;

        return;
    }

    public void LaunchException()
    {
        switch(exceptionType)
        {
            case AIStateExceptionType.NoAction:
                break;
            case AIStateExceptionType.NoStateMachine:
                break;
            case AIStateExceptionType.NoTransition:
                break;
            default:
                Debug.LogWarning("AISTate exception.");
                break;
        }

        return;
    }

    public void NoActionException()
    {
        if(stateWithException != null)
            Debug.LogWarning(stateWithException.GetStateName + " has no actions to execute.");
        else
            Debug.LogWarning("AISTate with no actions to execute.");

        return;
    }

    public void NoTransitionException()
    {
        if (stateWithException != null)
            Debug.LogWarning(stateWithException.GetStateName + " has no transitions for a AIStateMachine to transition.");
        else
            Debug.LogWarning("AISTate with no transitions for a AIStateMachine to transition.");
    }

    public void NotFoundTargetStateMachine()
    {
        if (stateWithException != null)
            Debug.LogWarning(stateWithException.GetStateName + "  no target AIStateMachine found.");
        else
            Debug.LogWarning("AISTate with no no target AIStateMachine found.");
    }
}
