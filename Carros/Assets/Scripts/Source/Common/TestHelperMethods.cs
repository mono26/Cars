using System.Reflection;
using UnityEngine;

public static class TestHelperMethods
{
    public static T CreateInitializedScriptInstanceInGameObject<T>(string _nameOfTest) where T : MonoBehaviour
    {
        T scriptTestInstanceInitialized = CreateTestScriptInstanceInGameObject<T>(_nameOfTest);
        InitializeTestGameObject(scriptTestInstanceInitialized.gameObject);
        return scriptTestInstanceInitialized;
    }

    public static T CreateTestScriptInstanceInGameObject<T>(string _nameOfTest) where T : MonoBehaviour
    {
        GameObject testGameObject = new GameObject("Test_ScriptInstance_" + typeof(T).Name+ "_" + _nameOfTest);
        T testScriptInstance = testGameObject.AddComponent<T>();
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
            CallMethod(behaviour, "Awake");
        }
        return;
    }

    public static void CallMethod(Object _behaviourToCallMethodFrom, string _methodName)
    {
        BindingFlags methodFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        MethodInfo methodToCall = _behaviourToCallMethodFrom.GetType().GetMethod(_methodName, methodFlag);
        if(methodToCall != null) {
            methodToCall.Invoke(_behaviourToCallMethodFrom, null);
        }
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
