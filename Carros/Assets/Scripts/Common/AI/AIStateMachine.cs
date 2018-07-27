using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : MonoBehaviour
{
    protected AIState currentState;

    public void ChangeState(AIState _newState)
    {
        currentState = _newState;

        return;
    }
    public void UpdateState()
    {
        return;
    }
}
