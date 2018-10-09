// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Reflection;
using UnityEngine;

public static class TestsHelperMethods
{
    public static T CreateInitializedScriptInstanceInGameObject<T>(string _nameOfTest) where T : MonoBehaviour
    {
        T scriptInstanceToInitialize = CreateScriptInstanceInGameObject<T>(_nameOfTest);
        InitializeTestGameObject(scriptInstanceToInitialize.gameObject);
        return scriptInstanceToInitialize;
    }

    public static T CreateScriptInstanceInGameObject<T>(string _nameOfTest) where T : MonoBehaviour
    {
        GameObject gameObject = new GameObject("Test_ScriptInstance_" + typeof(T).Name+ "_" + _nameOfTest);
        T testScriptInstance = gameObject.AddComponent<T>();
        return testScriptInstance;
    }

    public static void InitializeTestGameObject(GameObject _gameObjectToInitialize)
    {
        CallAllAwake(_gameObjectToInitialize);
        CallAllStart(_gameObjectToInitialize);
        CallAllOnEnable(_gameObjectToInitialize);
        return;
    }

    public static void CallAllAwake(GameObject _gameObjectToAwake)
    {
        foreach(MonoBehaviour behaviour in _gameObjectToAwake.GetComponentsInChildren<MonoBehaviour>()) {
            CallMethod(behaviour, "Awake", null);
        }
        return;
    }

    public static void CallMethod(MonoBehaviour _behaviourToCallMethodFrom, string _methodName, object[] _parameters)
    {
        BindingFlags methodFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo methodToCall = _behaviourToCallMethodFrom.GetType().GetMethod(_methodName, methodFlag);
        if(methodToCall != null) {
            methodToCall.Invoke(_behaviourToCallMethodFrom, _parameters);
        }
        return;
    }

    public static void CallAllStart(GameObject _gameObjectToStart)
    {
        foreach (MonoBehaviour behaviour in _gameObjectToStart.GetComponentsInChildren<MonoBehaviour>()) {
            CallMethod(behaviour, "Start", null);
        }
        return;
    }

    public static void CallAllOnEnable(GameObject _gameObjectToOnEnable)
    {
        foreach (MonoBehaviour behaviour in _gameObjectToOnEnable.GetComponentsInChildren<MonoBehaviour>()) {
            CallMethod(behaviour, "OnEnable", null);
        }
        return;
    }
}
