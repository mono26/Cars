// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

[System.Serializable]
public class AITransition
{
    [Header("AI Transitionsettings")]
    [SerializeField] protected AIDecision decision;
    [SerializeField] protected AIState trueState;
    [SerializeField] protected AIState falseState;

    public AIDecision GetDecision { get { return decision; } }
    public AIState GetTrueState { get { return trueState;}}
    public AIState GetFalseState {get { return falseState;}}
}