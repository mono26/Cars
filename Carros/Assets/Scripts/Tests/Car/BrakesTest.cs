using System.Reflection;
using UnityEngine;
using NUnit.Framework;

public class BrakesTest
{
    [Test]
    public void CreatesTestBrakes()
    {
        Brakes testBrakes = TestsHelperMethods.CreateScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testBrakes);
        return;
    }

    [Test]
    public void InitializeTestBrakes()
    {
        Brakes testBrakes = TestsHelperMethods.CreateScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        ReflectionBehaviour testReflection = testBrakes.gameObject.AddComponent<ReflectionBehaviour>();
        TestsHelperMethods.InitializeTestGameObject(testBrakes.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }

    [Test]
    public void GetRealFootBrakeTorqueValue()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(0f);
        Assert.NotNull(testFootBrakeTorque);
        return;
    }

    [Test]
    public void GetRealHandBrakeTorqueValue()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(0f);
        Assert.NotNull(testHandBrakeTorque);
        return;
    }

    [Test]
    public void ZeroFootBrakeTorqueWhenNoInputApplied()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(0);
        Assert.IsTrue(testFootBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void ZeroHandBrakeTorqueWhenNoInputApplied()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(0);
        Assert.IsTrue(testHandBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void GreaterThanZeroFootBrakeTorqueWhenInputApplied()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(1);
        Assert.Greater(testFootBrakeTorque, 0);
        return;
    }

    [Test]
    public void GreaterThanZeroHandBrakeTorqueWhenInputApplied()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(1);
        Assert.Greater(testHandBrakeTorque, 0);
        return;
    }

    [Test]
    public void NeverNegativeFootBrakeTorqueWhenNegativeInputApplied()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testFootBrakeTorque = testBrakes.GetFootBrakeTorqueToApply(-1);
        Assert.IsTrue(testFootBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void NeverNegativeHandBrakeTorqueWhenNegativeInputApplied()
    {
        Brakes testBrakes = TestsHelperMethods.CreateInitializedScriptInstanceInGameObject<Brakes>(MethodBase.GetCurrentMethod().Name);
        float testHandBrakeTorque = testBrakes.GetHandBrakeForceToApply(-1);
        Assert.IsTrue(testHandBrakeTorque.Equals(0));
        return;
    }
}
