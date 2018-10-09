using UnityEngine;
using NUnit.Framework;

public class TestsHelperMethodsTest
{
    [Test]
    public void CreatesTestGameObject()
    {
        ReflectionBehaviour testScriptInstanceGameObject = TestsHelperMethods.CreateScriptInstanceInGameObject<ReflectionBehaviour>("CreatesGameObject");
        Assert.NotNull(testScriptInstanceGameObject);
        return;
    }

    [Test]
    public void CreatesTestGameObjectWithCustomName()
    {
        ReflectionBehaviour testScriptInstanceGameObject = TestsHelperMethods.CreateScriptInstanceInGameObject<ReflectionBehaviour>("CreatesGameObjectWithCustomName");
        Assert.AreEqual(testScriptInstanceGameObject.gameObject.name, "Test_ScriptInstance_ReflectionBehaviour_CreatesGameObjectWithCustomName");
        return;
    }

    [Test]
    public void CallAwakeMethods()
    {
        ReflectionBehaviour testReflectionBehaviour = TestsHelperMethods.CreateScriptInstanceInGameObject<ReflectionBehaviour>("CallAwakeMethods");
        TestsHelperMethods.CallAllAwake(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.AwakeCalled);
        return;
    }

    [Test]
    public void CallStartMethods()
    {
        ReflectionBehaviour testReflectionBehaviour = TestsHelperMethods.CreateScriptInstanceInGameObject<ReflectionBehaviour>("CallStartMethods");
        TestsHelperMethods.CallAllStart(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.StartCalled);
        return;
    }

    [Test]
    public void CallOnEnableMethodsInTestGameObject()
    {
        ReflectionBehaviour testReflectionBehaviour = TestsHelperMethods.CreateScriptInstanceInGameObject<ReflectionBehaviour>("CallOnEnableMethodsInTestGameObject");
        TestsHelperMethods.CallAllOnEnable(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.OnEnableCalled);
        return;
    }

    [Test]
    public void CallCustomMethodInTestGameObject()
    {
        ReflectionBehaviour testReflectionBehaviour = TestsHelperMethods.CreateScriptInstanceInGameObject<ReflectionBehaviour>("CallCustomMethodInTestGameObject");
        TestsHelperMethods.CallMethod(testReflectionBehaviour, "CustomMethod", null);
        Assert.IsTrue(testReflectionBehaviour.CustomCalled);
        return;
    }

    [Test]
    public void InitializeTestGameObject()
    {
        ReflectionBehaviour testReflectionBehaviour = TestsHelperMethods.CreateScriptInstanceInGameObject<ReflectionBehaviour>("InitializeTestGameObject");
        TestsHelperMethods.InitializeTestGameObject(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.AwakeCalled);
        Assert.IsTrue(testReflectionBehaviour.StartCalled);
        Assert.IsTrue(testReflectionBehaviour.OnEnableCalled);
        return;
    }

    [Test]
    public void CreatesInitializedTestScriptInGameObject()
    {
        ReflectionBehaviour testReflectionBehaviourInstanceInGameObject = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<ReflectionBehaviour>("CreatesInitializedTestScriptInGameObject");
        Assert.IsTrue(testReflectionBehaviourInstanceInGameObject.AwakeCalled);
        Assert.IsTrue(testReflectionBehaviourInstanceInGameObject.StartCalled);
        Assert.IsTrue(testReflectionBehaviourInstanceInGameObject.OnEnableCalled);
        return;
    }

    [Test]
    public void CreatesInitializedTestScriptInGameObjectWithCustomName()
    {
        ReflectionBehaviour testReflectionBehaviourInstanceInGameObject = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<ReflectionBehaviour>("CreatesInitializedTestScriptInGameObjectWithCustomName");
        Assert.AreEqual(testReflectionBehaviourInstanceInGameObject.gameObject.name, "Test_ScriptInstance_ReflectionBehaviour_CreatesInitializedTestScriptInGameObjectWithCustomName");
        return;
    }
}
