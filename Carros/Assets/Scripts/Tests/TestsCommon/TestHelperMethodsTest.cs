using UnityEngine;
using NUnit.Framework;

public class TestHelperMethodsTest {

    [Test]
    public void CreatesTestGameObject()
    {
        ReflectionBehaviour testScriptInstanceGameObject = TestHelperMethods.CreateTestScriptInstanceInGameObject<ReflectionBehaviour>("CreatesGameObject");
        Assert.NotNull(testScriptInstanceGameObject);
        return;
    }

    [Test]
    public void CreatesTestGameObjectWithCustomName()
    {
        ReflectionBehaviour testScriptInstanceGameObject = TestHelperMethods.CreateTestScriptInstanceInGameObject<ReflectionBehaviour>("CreatesGameObjectWithCustomName");
        Assert.AreEqual(testScriptInstanceGameObject.gameObject.name, "Test_ScriptInstance_ReflectionBehaviour_CreatesGameObjectWithCustomName");
        return;
    }

    [Test]
    public void CallAwakeMethods()
    {
        ReflectionBehaviour testReflectionBehaviour = TestHelperMethods.CreateTestScriptInstanceInGameObject<ReflectionBehaviour>("CallAwakeMethods");
        TestHelperMethods.CallAllAwake(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.AwakeCalled);
        return;
    }

    [Test]
    public void CallStartMethods()
    {
        ReflectionBehaviour testReflectionBehaviour = TestHelperMethods.CreateTestScriptInstanceInGameObject<ReflectionBehaviour>("CallStartMethods");
        TestHelperMethods.CallAllStart(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.StartCalled);
        return;
    }

    [Test]
    public void CallOnEnableMethodsInTestGameObject()
    {
        ReflectionBehaviour testReflectionBehaviour = TestHelperMethods.CreateTestScriptInstanceInGameObject<ReflectionBehaviour>("CallOnEnableMethodsInTestGameObject");
        TestHelperMethods.CallAllOnEnable(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.OnEnableCalled);
        return;
    }

    [Test]
    public void CallCustomMethodInTestGameObject()
    {
        ReflectionBehaviour testReflectionBehaviour = TestHelperMethods.CreateTestScriptInstanceInGameObject<ReflectionBehaviour>("CallCustomMethodInTestGameObject");
        TestHelperMethods.CallMethod(testReflectionBehaviour, "CustomMethod");
        Assert.IsTrue(testReflectionBehaviour.CustomCalled);
        return;
    }

    [Test]
    public void InitializeTestGameObject()
    {
        ReflectionBehaviour testReflectionBehaviour = TestHelperMethods.CreateTestScriptInstanceInGameObject<ReflectionBehaviour>("InitializeTestGameObject");
        TestHelperMethods.InitializeTestGameObject(testReflectionBehaviour.gameObject);
        Assert.IsTrue(testReflectionBehaviour.AwakeCalled);
        Assert.IsTrue(testReflectionBehaviour.StartCalled);
        Assert.IsTrue(testReflectionBehaviour.OnEnableCalled);
        return;
    }

    [Test]
    public void CreatesInitializedTestScriptInGameObject()
    {
        ReflectionBehaviour testReflectionBehaviourInstanceInGameObject = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<ReflectionBehaviour>("CreatesInitializedTestScriptInGameObject");
        Assert.IsTrue(testReflectionBehaviourInstanceInGameObject.AwakeCalled);
        Assert.IsTrue(testReflectionBehaviourInstanceInGameObject.StartCalled);
        Assert.IsTrue(testReflectionBehaviourInstanceInGameObject.OnEnableCalled);
        return;
    }

    [Test]
    public void CreatesInitializedTestScriptInGameObjectWithCustomName()
    {
        ReflectionBehaviour testReflectionBehaviourInstanceInGameObject = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<ReflectionBehaviour>("CreatesInitializedTestScriptInGameObjectWithCustomName");
        Assert.AreEqual(testReflectionBehaviourInstanceInGameObject.gameObject.name, "Test_ScriptInstance_ReflectionBehaviour_CreatesInitializedTestScriptInGameObjectWithCustomName");
        return;
    }
}
