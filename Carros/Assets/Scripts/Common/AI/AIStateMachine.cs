using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    [Header("AIState Machine components")]
    [SerializeField]
    protected Entity aiEntity;

    [Header("Editor debugging")]
    [SerializeField]
    protected AIState currentState;

    protected void Awake()
    {
        if (aiEntity == null)
            GetComponent<Entity>();

        return;   
    }

    public void ChangeState(AIState _newState)
    {
        currentState = _newState;

        return;
    }

    public void UpdateState()
    {
        currentState.UpdateState(aiEntity);

        return;
    }
}
