using System.Reflection;
using UnityEngine;
using NUnit.Framework;

public class WheelTest
{
    [Test]
    public void CreatesTestWheel()
    {
        Wheel testBrakes = TestHelperMethods.CreateScriptInstanceInGameObject<Wheel>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testBrakes);
        return;
    }

    [Test]
    public void InitializeTestWheel()
    {
        Wheel testBrakes = TestHelperMethods.CreateScriptInstanceInGameObject<Wheel>(MethodBase.GetCurrentMethod().Name);
        ReflectionBehaviour testReflection = testBrakes.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testBrakes.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }
}
