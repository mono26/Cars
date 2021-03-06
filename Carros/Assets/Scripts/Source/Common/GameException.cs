﻿// Copyright (c) What a Box Creative Studio. All rights reserved.

using System;
using UnityEngine;

public class GameException : Exception
{
    public GameException() : base() { }
    public GameException(string _message) : base(_message) { }

    protected virtual string FormatMessage()
    {
        string message;
        if (Message == null || Message == "") {
            message = "The game encountered an exception.";
        }
        else {
            message = Message;
        }
        return message;
    }

    private void LogMessage(string _messageToLog)
    {
        Debug.LogWarning(_messageToLog);
        return;
    }

    public void DisplayException()
    {
        string messageToLog = FormatMessage();
        LogMessage(messageToLog);
        return;
    }
}

public class MissingComponentException : GameException
{
    private Type missingComponent;
    private GameObject gameObjectWithException;

    public MissingComponentException() : base("Missing component exception.") { }
    public MissingComponentException(string _message) : base(_message) { }
    // TODO refactorization when we only pass a gameObject.
    public MissingComponentException(GameObject _gameObjectWithException, Type _missingComponent) : base()
    {
        missingComponent = _missingComponent;
        gameObjectWithException = _gameObjectWithException;
        return;
    }

    protected override string FormatMessage()
    {
        string message;
        if (Message == null || Message == "")
            message = "The GameObject " + gameObjectWithException.name + " has a missing component: " + missingComponent.ToString();
        else
            message = Message + missingComponent.ToString();
        return message;
    }
}

public class EventException : GameException
{
    Type eventType;

    public EventException() : base("Event Exception") { }
    public EventException(Type _eventType) : base()
    {
        eventType = _eventType;
        return;
    }
    public EventException(Type _eventType, string _message) : base(_message)
    {
        eventType = _eventType;
        return;
    }

    protected override string FormatMessage()
    {
        string message;
        if (Message == null || Message == "")
            message = "Event of type: " + eventType + " encountered a exception.";
        else
            message = eventType.Name + Message;
        return message;
    }
}

public class AIStateException : GameException
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
        switch (exceptionType)
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
        if (stateWithException != null)
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
