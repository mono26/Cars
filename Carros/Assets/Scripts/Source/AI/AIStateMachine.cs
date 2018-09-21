using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    [Header("AIState Machine components")]
    [SerializeField] private Entity aIEntity = null;

    [Header("AIState Machine editor debugging")]
    [SerializeField] private AIState currentState = null;

    public AIState GetCurrentState { get { return currentState; } }

    private void Awake()
    {
        if (aIEntity == null)
            GetComponent<Entity>();

        return;   
    }

    private bool HasAIEntityToUpdateState()
    {
        bool hasAIEntity = true;
        if (aIEntity == null)
        {
            hasAIEntity = false;
            throw new MissingComponentException("The state machine has no Entity to Update.", typeof(Entity));
        }
        return hasAIEntity;
    }

    public void UpdateState()
    {
        try
        {
            if (HasAIEntityToUpdateState()) {
                currentState.UpdateState(aIEntity);
            }
        }
        catch (MissingComponentException missingComponentException)
        {
            missingComponentException.DisplayException();
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
