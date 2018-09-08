using System;
using UnityEngine;

public class GameException : Exception
{
    public GameException() : base() { }
    public GameException(string _message) : base(_message) { }

    public void DisplayException()
    {
        string messageToLog = FormatMessage();
        LogMessage(messageToLog);
        return;
    }

    protected virtual string FormatMessage()
    {
        string message;
        if (Message == null || Message == "")
            message = "The game encountered an exception.";
        else
            message = Message;
        return message;
    }

    private void LogMessage(string _messageToLog)
    {
        Debug.LogWarning(_messageToLog);
        return;
    }
}

public class MissingComponentException : GameException
{
    private Type missingComponent;
    private Type GetMissingComponent { get { return missingComponent; } }

    public MissingComponentException() : base() { }
    public MissingComponentException(string _message) : base(_message) { }
    public MissingComponentException(string _message, Type _missingComponent) : base(_message)
    {
        missingComponent = _missingComponent;
    }

    protected override string FormatMessage()
    {
        string message;
        if (Message == null || Message == "")
            message = "Object has a missing component: ";
        else
            message = Message;
        return message + missingComponent.ToString();
    }
}
