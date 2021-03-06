﻿// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ExternalInput : EntityComponent
{
    [Header("External Input settings")]
    [SerializeField] protected string playerID;

    [Header("Editor debugging")]
    [SerializeField] protected float aimInput;
    [SerializeField] protected float footBrakesInput;
    [SerializeField] protected float handBrakesInput;
    [SerializeField] protected float movementInput;
    [SerializeField] protected float steeringInput;

    public float GetAimInput { get { return aimInput; } }
    public float GetFootBrakesInput { get { return footBrakesInput; } }
    public float GetHandBrakeInput { get { return handBrakesInput; } }
    public float GetMovementInput { get { return movementInput; } }
    public float GetSteeringInput { get { return steeringInput; } }

    public override void EveryFrame()
    {
        GetTheInput();
        return;
    }

    protected virtual void GetTheInput()
    {
        aimInput = CrossPlatformInputManager.GetAxis("Aim");
        footBrakesInput = CrossPlatformInputManager.GetAxis("Vertical");
        // TODO check if its better to dot with a button.
        handBrakesInput = CrossPlatformInputManager.GetAxis("HandBrake");
        movementInput = CrossPlatformInputManager.GetAxis("Vertical");
        steeringInput = CrossPlatformInputManager.GetAxis("Horizontal");
        return;
    }
}
