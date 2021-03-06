﻿// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyAnimationParameters
{
    private EnemyStateInTheWorld currentStateInTheWorld;
    private EnemyMovemenState currentMovementState;
    private float speedPercentage;

    public EnemyStateInTheWorld GetCurrentStateInTheWorld { get { return currentStateInTheWorld; } }
    public EnemyMovemenState GetCurrentMovementState { get { return currentMovementState; } }
    public float GetSpeedPercentage { get { return speedPercentage; } }

    public EnemyAnimationParameters(EnemyStateInTheWorld _currentStateInTheWorld, EnemyMovemenState _currentMovementState, float _speedPercentage)
    {
        currentStateInTheWorld = _currentStateInTheWorld;
        currentMovementState = _currentMovementState;
        speedPercentage = _speedPercentage;
        return;
    }
}

public class EnemyAnimationComponent : EntityComponent
{
    private void HandleMovementAnimations(EnemyMovemenState _enemyMovementState)
    {
        Animator animatorToHandle = entity.GetAnimatorComponent;
        if (animatorToHandle == null) { return; }
        animatorToHandle.SetBoolParameterIfExisting(
            "IsIdle",
            AnimatorControllerParameterType.Bool,
            _enemyMovementState == EnemyMovemenState.Idle
            );
        animatorToHandle.SetBoolParameterIfExisting(
            "IsRunning",
            AnimatorControllerParameterType.Bool,
            _enemyMovementState == EnemyMovemenState.Running
            );
        animatorToHandle.SetBoolParameterIfExisting(
            "IsWalking",
            AnimatorControllerParameterType.Bool,
            _enemyMovementState == EnemyMovemenState.Walking
            );
        return;
    }

    private void HandleEnemyStatesAniamtions(EnemyStateInTheWorld _enemyStateInTheWorld)
    {
        Animator animatorToHandle = entity.GetAnimatorComponent;
        if (animatorToHandle == null) { return; }
        animatorToHandle.SetBoolParameterIfExisting(
            "IsGrounded",
            AnimatorControllerParameterType.Bool,
            _enemyStateInTheWorld == EnemyStateInTheWorld.Grounded
            );
        return;
    }

    private void HandleAnimationSpeed(float _movementSpeedPercent)
    {

    }

    public void HandleAnimations(EnemyAnimationParameters _enemyAnimationParameters)
    {
        HandleEnemyStatesAniamtions(_enemyAnimationParameters.GetCurrentStateInTheWorld);
        HandleMovementAnimations(_enemyAnimationParameters.GetCurrentMovementState);
        HandleAnimationSpeed(_enemyAnimationParameters.GetSpeedPercentage);
        return;
    }
}
