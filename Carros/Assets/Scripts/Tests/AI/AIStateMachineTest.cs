using System.Reflection;
using UnityEngine;
using NUnit.Framework;

public class AIStateMachineTest
{
    private string testSCOAIStateName = "SCO_AI_STT_TestState";

    [Test]
    public void CreatesTestAIStateMachine()
    {
        AIStateMachine testAIStateMachine = TestHelperMethods.CreateScriptInstanceInGameObject<AIStateMachine>(MethodBase.GetCurrentMethod().Name);
        Assert.NotNull(testAIStateMachine);
        return;
    }

    [Test]
    public void InitializeTestAIStateMachine()
    {
        AIStateMachine testAIStateMachine = TestHelperMethods.CreateScriptInstanceInGameObject<AIStateMachine>(MethodBase.GetCurrentMethod().Name);
        ReflectionBehaviour testReflection = testAIStateMachine.gameObject.AddComponent<ReflectionBehaviour>();
        TestHelperMethods.InitializeTestGameObject(testAIStateMachine.gameObject);
        Assert.IsTrue(testReflection.AwakeCalled);
        Assert.IsTrue(testReflection.StartCalled);
        Assert.IsTrue(testReflection.OnEnableCalled);
        return;
    }

    [Test]
    public void StateMachineStartsWithNoCurrentState()
    {
        AIStateMachine testAIStateMachine = TestHelperMethods.CreateScriptInstanceInGameObject<AIStateMachine>(MethodBase.GetCurrentMethod().Name);
        TestHelperMethods.InitializeTestGameObject(testAIStateMachine.gameObject);
        Assert.IsNull(testAIStateMachine.GetCurrentState);
        return;
    }

    [Test]
    public void StateMachineChangesToNotNullState()
    {
        AIStateMachine testAIStateMachine = TestHelperMethods.CreateScriptInstanceInGameObject<AIStateMachine>(MethodBase.GetCurrentMethod().Name);
        TestHelperMethods.InitializeTestGameObject(testAIStateMachine.gameObject);
        AIState testState = Resources.Load<AIState>("TestResources/" + testSCOAIStateName);
        testAIStateMachine.ChangeState(testState);
        Assert.NotNull(testAIStateMachine.GetCurrentState);
        return;
    }

    [Test]
    public void StateMachineChangesStateAndMatchesNames()
    {
        AIStateMachine testAIStateMachine = TestHelperMethods.CreateScriptInstanceInGameObject<AIStateMachine>(MethodBase.GetCurrentMethod().Name);
        TestHelperMethods.InitializeTestGameObject(testAIStateMachine.gameObject);
        AIState testState = Resources.Load<AIState>("TestResources/" + testSCOAIStateName);
        testAIStateMachine.ChangeState(testState);
        Assert.AreEqual(testAIStateMachine.GetCurrentState.GetStateName, testState.GetStateName);
        return;
    }
}
