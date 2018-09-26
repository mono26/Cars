using UnityEngine;

public class AIStateMachine : EntityComponent
{
    [Header("AIState Machine editor debugging")]
    [SerializeField] private AIState currentState = null;

    public AIState GetCurrentState { get { return currentState; } }

    private bool HasEntityToUpdateState()
    {
        bool hasAIEntity = true;
        try
        {
            if (entity == null)
            {
                hasAIEntity = false;
                throw new MissingComponentException("The state machine has no Entity to Update.", typeof(Entity));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasAIEntity;
    }

    public void UpdateState()
    {
        if (HasEntityToUpdateState()) {
            currentState.UpdateState(entity);
        }
        return;
    }

    public void ChangeState(AIState _newState)
    {
        if (_newState != null && !_newState.GetStateName.Equals("RemainInState")) {
            currentState = _newState;
        }
        return;
    }
}
