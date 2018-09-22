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
    public void ReserveAttackingSlot()
    {
        SlotManager testSlotManager = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<SlotManager>(MethodBase.GetCurrentMethod().Name);
        GameObject testAttacker = new GameObject();
        Slot testAttackerSlot = testSlotManager.Reserve(testAttacker);
        Assert.NotNull(testAttackerSlot);
        Assert.AreEqual(testAttackerSlot.GetSlotType, SlotType.Attacking);
        Assert.NotNull(testSlotManager.GetAttackingSlots[testAttackerSlot.GetIndex]);
        return;
    }

    [Test]
    public void ReserveWaitingSlot()
    {
        SlotManager testSlotManager = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<SlotManager>(MethodBase.GetCurrentMethod().Name);
        // Default number of slots.
        Slot[] attackerSlots = new Slot[5];
        for (int i = 0; i < 5; i++) {
            GameObject testAttacker = new GameObject();
            attackerSlots[i] = testSlotManager.Reserve(testAttacker);
        }
        GameObject testWaiter = new GameObject();
        Slot testWaiterSlot = testSlotManager.Reserve(testWaiter);
        foreach(Slot attackerSlot in attackerSlots)
        {
            Assert.NotNull(attackerSlot);
            Assert.AreEqual(attackerSlot.GetSlotType, SlotType.Attacking);
            Assert.NotNull(testSlotManager.GetAttackingSlots[attackerSlot.GetIndex]);
        }
        Assert.NotNull(testWaiterSlot);
        Assert.AreEqual(testWaiterSlot.GetSlotType, SlotType.Waiting);
        Assert.NotNull(testSlotManager.GetWaitingSlots[testWaiterSlot.GetIndex]);
        return;
    }

    [Test]
    public void ReleaseAttackingSlot()
    {
        SlotManager testSlotManager = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<SlotManager>(MethodBase.GetCurrentMethod().Name);
        GameObject testAttacker = new GameObject();
        Slot testSlot = testSlotManager.Reserve(testAttacker);
        testSlotManager.Release(testSlot);
        Assert.IsNull(testSlotManager.GetAttackingSlots[testSlot.GetIndex]);
        return;
    }
}
