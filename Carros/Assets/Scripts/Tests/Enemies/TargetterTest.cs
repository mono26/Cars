using System.Reflection;
using UnityEngine;
using NUnit.Framework;

public class TargetterTest
{
    [Test]
    public void CreatesTestTargetter()
    {
        Targetter testTargetter = TestHelperMethods.CreateScriptInstanceInGameObject<Targetter>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testTargetter);
        return;
    }

    [Test]
    public void InitializeTestTargetter()
    {
        Targetter testTargetter = TestHelperMethods.CreateScriptInstanceInGameObject<Targetter>(MethodBase.GetCurrentMethod().Name);
        testTargetter.gameObject.AddComponent<SphereCollider>();
        ReflectionBehaviour testReflection = testTargetter.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testTargetter.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }
}
