using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public static class TestHelperMethods
{
    private static List<GameObject> testObjects = new List<GameObject>();

    public static GameObject CreateTestGameObject()
    {
        GameObject testGameObject = new GameObject("Test");
        testObjects.Add(testGameObject);
        return testGameObject;
    }

    public static void ClearTestObjects()
    {
        foreach(GameObject testObject in testObjects) {
            GameObject.Destroy(testObject);
        }
        testObjects.Clear();
        return;
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
            CallMethod(behaviour, "Awake");
        }
        return;
    }

    public static void CallMethod(Object _objectToCallMethod, string _methodName)
    {
        BindingFlags methodFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        var methodToCall = _objectToCallMethod.GetType().GetMethod(_methodName, methodFlag);
        methodToCall.Invoke(_objectToCallMethod, null);
        return;
    }

    public static void CallAllStart(GameObject _gameObjectToStart)
    {
        foreach (MonoBehaviour behaviour in _gameObjectToStart.GetComponentsInChildren<MonoBehaviour>()) {
            CallMethod(behaviour, "Start");
        }
        return;
    }

    public static void CallAllOnEnable(GameObject _gameObjectToOnEnable)
    {
        foreach (MonoBehaviour behaviour in _gameObjectToOnEnable.GetComponentsInChildren<MonoBehaviour>()) {
            CallMethod(behaviour, "OnEnable");
        }
        return;
    }
}
