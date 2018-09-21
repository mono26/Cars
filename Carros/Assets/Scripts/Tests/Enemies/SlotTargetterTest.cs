using System.Reflection;
using UnityEngine;
using NUnit.Framework;

public class SlotTargetterTest
{
    [Test]
    public void CreatesTestSlotTargetter()
    {
        SlotTargetter testSlotTargetter = TestHelperMethods.CreateScriptInstanceInGameObject<SlotTargetter>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testSlotTargetter);
        return;
    }

    [Test]
    public void InitializeTestSlotTargetter()
    {
        SlotTargetter testSlotTargetter = TestHelperMethods.CreateScriptInstanceInGameObject<SlotTargetter>(MethodBase.GetCurrentMethod().Name);
        testSlotTargetter.gameObject.AddComponent<SphereCollider>();
        ReflectionBehaviour testReflection = testSlotTargetter.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testSlotTargetter.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }

    /*[Test]
    public void AddsSlotManagerTargetAndSlot()
    {
        SlotTargetter testSlotTargetter = TestHelperMethods.CreateTestScriptInstanceInGameObject<SlotTargetter>("AddsSlotManagerTargetAndSlot");
        testSlotTargetter.gameObject.AddComponent<Entity>();
        testSlotTargetter.gameObject.AddComponent<SphereCollider>();
        GameObject testGameObject = new GameObject {
            tag = "Player"
        };
        testGameObject.AddComponent<SlotManager>();
        Collider testCollider = testGameObject.AddComponent<BoxCollider>();
        TestHelperMethods.InitializeTestGameObject(testSlotTargetter.gameObject);
        TestHelperMethods.InitializeTestGameObject(testGameObject);
        Collider[] testParameters = new Collider[] { testCollider };
        TestHelperMethods.CallMethod(testSlotTargetter, "OnTriggerEnter", testParameters);
        Assert.NotNull(testSlotTargetter.GetCurrentSlotManagerTarget);
    }*/
}
