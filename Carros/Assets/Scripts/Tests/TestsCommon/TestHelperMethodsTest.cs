using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TestHelperMethodsTest {

    [Test]
    public void CreatesGameObject()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        Assert.NotNull(testGameObject);
        return;
    }

    [Test]
    public void CallAwakeMethods()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        ReflectionBehaviour reflectionTestGameObject = testGameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.CallAllAwake(reflectionTestGameObject.gameObject);
        Assert.IsTrue(reflectionTestGameObject.AwakeCalled);
        return;
    }

    [Test]
    public void CallStartMethods()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        ReflectionBehaviour reflectionTestGameObject = testGameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.CallAllStart(reflectionTestGameObject.gameObject);
        Assert.IsTrue(reflectionTestGameObject.StartCalled);
        return;
    }

    [Test]
    public void CallOnEnableMethods()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        ReflectionBehaviour reflectionTestGameObject = testGameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.CallAllOnEnable(reflectionTestGameObject.gameObject);
        Assert.IsTrue(reflectionTestGameObject.OnEnableCalled);
        return;
    }

    [Test]
    public void InitializeTestGameObject()
    {
        GameObject testGameObject = TestHelperMethods.CreateTestGameObject();
        ReflectionBehaviour reflectionTestGameObject = testGameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(reflectionTestGameObject.gameObject);
        Assert.IsTrue(reflectionTestGameObject.AwakeCalled);
        Assert.IsTrue(reflectionTestGameObject.StartCalled);
        Assert.IsTrue(reflectionTestGameObject.OnEnableCalled);
        return;
    }

    /*[Test]
    public void TestHelperMethodsTestSimplePasses() {
        // Use the Assert class to test conditions.
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TestHelperMethodsTestWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }*/
}
