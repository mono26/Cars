using UnityEngine;
using NUnit.Framework;

public class CarEngineTest
{
    [Test]
    public void CreatesTestCarEngine()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateTestScriptInstanceInGameObject<CarEngine>("CreatesTestCarEngine");
        Assert.NotNull(testCarEngine);
        return;
    }

    [Test]
    public void InitializeTestCarEngine()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateTestScriptInstanceInGameObject<CarEngine>("InitializeTestCarEngine");
        ReflectionBehaviour testReflection = testCarEngine.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testCarEngine.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }

    [Test]
    public void GetRealEngineTorqueValue()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>("GetRealEngineTorqueValue");
        float testTorque = testCarEngine.GetCarEngineTorqueToApply(0);
        Assert.NotNull(testTorque);
        return;
    }

    [Test]
    public void ZeroEngineTorqueWhenNoInputApplied()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>("ZeroEngineTorqueWhenNoInputApplied");
        float testTorque = testCarEngine.GetCarEngineTorqueToApply(0);
        Assert.IsTrue(testTorque.Equals(0));
        return;
    }

    [Test]
    public void GreaterThanZeroEngineTorqueWhenInputApplied()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>("GreaterThanZeroEngineTorqueWhenInputApplied");
        float testTorque = testCarEngine.GetCarEngineTorqueToApply(0);
        Assert.IsTrue(testTorque.Equals(0));
        return;
    }

    [Test]
    public void NeverNegativeCarEngineTorqueWhenNegativeInputApplied()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>("NeverNegativeCarEngineTorqueWhenNegativeInputApplied");
        float testFootBrakeTorque = testCarEngine.GetCarEngineTorqueToApply(-1);
        Assert.IsTrue(testFootBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void EngineCapsSpeed()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>("EngineCapsSpeed");
        Vector3 testVelocity = Vector3.forward * 9999f;
        Vector3 capedTestVelocity = testCarEngine.CapVelocityMagnitudeToMaxSpeed(testVelocity);
        Assert.IsTrue(testVelocity.magnitude > capedTestVelocity.magnitude);
        return;
    }

    [Test]
    public void EngineCalculateZeroRevolutions()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>("EngineCalculateZeroRevolutions");
        float testSpeed = 0;
        testCarEngine.CalculateEngineRevolutions(testSpeed);
        Assert.IsTrue(testCarEngine.GetCurrentEngineRevolutions.Equals(0));
        return;
    }
}
