using System.Reflection;
using UnityEngine;
using NUnit.Framework;

public class CarEngineTest
{
    [Test]
    public void CreatesTestCarEngine()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testCarEngine);
        return;
    }

    [Test]
    public void InitializeTestCarEngine()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
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
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        float testTorque = testCarEngine.GetCarEngineTorqueToApply(0);
        Assert.NotNull(testTorque);
        return;
    }

    [Test]
    public void ZeroEngineTorqueWhenNoInputApplied()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        float testTorque = testCarEngine.GetCarEngineTorqueToApply(0);
        Assert.IsTrue(testTorque.Equals(0));
        return;
    }

    [Test]
    public void GreaterThanZeroEngineTorqueWhenInputApplied()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        float testTorque = testCarEngine.GetCarEngineTorqueToApply(0);
        Assert.IsTrue(testTorque.Equals(0));
        return;
    }

    [Test]
    public void NeverNegativeCarEngineTorqueWhenNegativeInputApplied()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        float testFootBrakeTorque = testCarEngine.GetCarEngineTorqueToApply(-1);
        Assert.IsTrue(testFootBrakeTorque.Equals(0));
        return;
    }

    [Test]
    public void EngineCapsSpeed()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        Vector3 testVelocity = Vector3.forward * 9999f;
        Vector3 capedTestVelocity = testCarEngine.CapVelocityMagnitudeToMaxSpeed(testVelocity);
        Assert.Greater(testVelocity.magnitude, capedTestVelocity.magnitude);
        return;
    }

    [Test]
    public void EngineCalculateZeroRevolutions()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        float testCarSpeed = 0;
        float testCarEngineRevolutions = testCarEngine.CalculateEngineRevolutions(testCarSpeed);
        Assert.IsTrue(testCarEngineRevolutions.Equals(0));
        return;
    }

    [Test]
    public void EngineCalculateMaxRevolutionsAtFullSpeed()
    {
        CarEngine testCarEngine = TestHelperMethods.CreateInitializedScriptInstanceInGameObject<CarEngine>(MethodBase.GetCurrentMethod().Name);
        float testCarSpeed = 150;
        float testCarEngineRevolutions = testCarEngine.CalculateEngineRevolutions(testCarSpeed);
        Assert.GreaterOrEqual(testCarEngineRevolutions, 1);
        return;
    }
}
