using System.Reflection;
using UnityEngine;
using NUnit.Framework;

public class SlotManagerTest
{
    [Test]
    public void CreatesTestSlotManager()
    {
        SlotManager testSlotManager = TestHelperMethods.CreateScriptInstanceInGameObject<SlotManager>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testSlotManager);
        return;
    }

    [Test]
    public void InitializeTestSlotManager()
    {
        SlotManager testSlotManager = TestHelperMethods.CreateScriptInstanceInGameObject<SlotManager>(MethodBase.GetCurrentMethod().Name);
        ReflectionBehaviour testReflection = testSlotManager.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testSlotManager.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }

    [Test]
    public void ReserveSlot()
    {
        SlotManager testSlotManager = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<SlotManager>(MethodBase.GetCurrentMethod().Name);
        GameObject testAttacker = new GameObject();
        SlotManager.Slot testSlot = testSlotManager.Reserve(testAttacker);
        Assert.NotNull(testSlot);
    }

    [Test]
    public void ReleaseSlot()
    {
        SlotManager testSlotManager = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<SlotManager>(MethodBase.GetCurrentMethod().Name);
        GameObject testAttacker = new GameObject();
        SlotManager.Slot testSlot = testSlotManager.Reserve(testAttacker);
        testSlotManager.Release(ref testSlot);
        Assert.Null(testSlot);
    }
}
