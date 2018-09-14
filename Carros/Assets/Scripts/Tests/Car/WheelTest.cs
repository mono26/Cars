using UnityEngine;
using NUnit.Framework;

public class WheelTest
{
    [Test]
    public void CreatesTestWheel()
    {
        Wheel testBrakes = TestHelperMethods.CreateTestScriptInstanceInGameObject<Wheel>("CreatesTestWheel");
        Assert.NotNull(testBrakes);
        return;
    }

    [Test]
    public void InitializeTestWheel()
    {
        Wheel testBrakes = TestHelperMethods.CreateTestScriptInstanceInGameObject<Wheel>("InitializeTestWheel");
        ReflectionBehaviour testReflection = testBrakes.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testBrakes.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }
}
