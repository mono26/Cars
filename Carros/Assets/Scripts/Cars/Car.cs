﻿using System;
using UnityEngine;

[RequireComponent(typeof(CarEngine), typeof(Brakes))]
public class Car : Entity
{
    [Header("Car settings")]
    [SerializeField] private Vector3 m_CentreOfMassOffset = Vector3.zero;
    [SerializeField] [Range(0, 1)] protected float directionAssist = 0.644f; // 0 is raw physics , 1 the car will grip in the direction it is facing.

    [Header("Car components")]
    [SerializeField] protected Brakes brakes;
    [SerializeField] protected CarEngine engine;
    [SerializeField] protected Wheel[] wheels;  // Assuming 0 and 1 are the front wheels and 2 and 3 the back wheels.

    [Header("Editor debugging")]
    [SerializeField] protected float accelerationInput;
    [SerializeField] protected float footBrakeInput;
    [SerializeField] protected float handBrakeInput;
    [SerializeField] protected float oldRotation;
    [SerializeField] protected float steeringInput;

    public CarEngine GetEngine { get { return engine; } }
    public Wheel[] GetWheels { get { return wheels; } }

    private bool AnySkidSoundPlaying()
    {
        /*for (int i = 0; i < 4; i++)
        {
            if (m_WheelEffects[i].PlayingAudio)
            {
                return true;
            }
        }*/
        return false;
    }

    protected override void Awake()
    {
        if (brakes == null)
            brakes = GetComponent<Brakes>();
        if (engine == null)
            engine = GetComponent<CarEngine>();
        if (wheels == null)
            wheels = GetComponentsInChildren<Wheel>();

        base.Awake();
    }

    protected void ApplyBrakes()
    {
        HandBrake();
        FootBrake();

        return;
    }

    protected void ApplyEngineTorqueToWheels()
    {
        try
        {
            if (engine == null) { throw new CarMissingComponentException("TheEngine"); }
            if (wheels == null) { throw new CarMissingComponentException("AlTheWheels"); }

            float accelerationTorque = engine.GetTorqueToApply(accelerationInput);
            foreach (Wheel wheel in wheels)
            {
                if (wheel == null) { { throw new CarMissingComponentException("AWheel"); } }
                wheel.SetTorque(accelerationTorque, Wheel.TorqueType.Acceleration);
            }
        }
        catch (CarMissingComponentException exception)
        {
            Debug.LogWarning(exception.Message + exception.GetMissingComponent);
        }

        return;
    }

    protected void DirectionAsssist()
    {
        try
        {
            if (wheels == null) { throw new CarMissingComponentException("AllTheWheels"); }

            foreach (Wheel wheelToCheck in wheels)
            {
                if (wheelToCheck == null) { { throw new CarMissingComponentException("AWheel"); } }
                WheelHit wheelhit;
                wheelToCheck.GetWheelCollider.GetGroundHit(out wheelhit);
                if (wheelhit.normal == Vector3.zero)
                    return; // wheels arent on the ground so dont realign the rigidbody velocity
            }
            // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            if (Mathf.Abs(oldRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - oldRotation) * directionAssist;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                body.velocity = velRotation * body.velocity;
            }
            oldRotation = transform.eulerAngles.y;
        }

        catch (CarMissingComponentException exception)
        {
            Debug.LogWarning(exception.Message + exception.GetMissingComponent);
        }

        return;
    }

    protected void Drive()
    {
        SteerFrontWheels();
        ApplyEngineTorqueToWheels();
        ApplyBrakes();

        return;
    }

    protected override void FixedUpdate()
    {
        DirectionAsssist();
        Drive();
        TractionControl();

        base.FixedUpdate();

        return;
    }

    protected void FootBrake()
    {
        try
        {
            if(brakes == null) { throw new CarMissingComponentException("Brakes"); }

            if (footBrakeInput > 0)
            {
                float footBrakeTorque = -brakes.GetFootBrakeForceToApply(footBrakeInput);
                foreach (Wheel wheel in wheels)
                {
                    if(wheel == null) { throw new CarMissingComponentException("AWheel"); }
                    wheel.SetTorque(footBrakeTorque, Wheel.TorqueType.Acceleration);
                }
            }
        }
        catch (CarMissingComponentException exception)
        {
            Debug.LogWarning(exception.Message + exception.GetMissingComponent);
        }

        return;
    }

    protected void HandleAccelerationInput()
    {
        if (!CanApplyExternalInputToEntity()) { return; }

        accelerationInput = input.GetMovementInput;
        accelerationInput = Mathf.Clamp(accelerationInput, 0, 1);

        return;
    }

    protected void HandBrake()
    {
        try
        {
            if (wheels[0] == null || wheels[1] == null) { throw new CarMissingComponentException("FrontWheels"); }

            if (handBrakeInput > 0)
            {
                float handBrakeTorque = brakes.GetHandBrakeForceToApply(handBrakeInput);
                wheels[0].SetTorque(handBrakeTorque, Wheel.TorqueType.Braking);
                wheels[1].SetTorque(handBrakeTorque, Wheel.TorqueType.Braking);
            }
        }
        catch (CarMissingComponentException exception)
        {
            Debug.LogWarning(exception.Message + exception.GetMissingComponent);
        }

        return;
    }

    protected void HandleBrakesInput()
    {
        if (!CanApplyExternalInputToEntity()) { return; }

        footBrakeInput = input.GetFootBrakesInput;
        footBrakeInput = -1 * Mathf.Clamp(footBrakeInput, -1, 0);
        handBrakeInput = input.GetHandBrakeInput;
        handBrakeInput = Mathf.Clamp(handBrakeInput, 0, 1);

        return;
    }

    protected void HandleInput()
    {
        if (!CanApplyExternalInputToEntity()) { return; }

        HandleAccelerationInput();
        HandleBrakesInput();
        HandleSteeringInput();

        return;
    }

    protected void HandleSteeringInput()
    {
        if (!CanApplyExternalInputToEntity()) { return; }

        steeringInput = input.GetSteeringInput;
        steeringInput = Mathf.Clamp(steeringInput, -1, 1);

        return;
    }

    protected void Start()
    {
        /*m_WheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++)
        {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }
        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;*/
    }

    protected void SteerFrontWheels()
    {
        try
        {
            if (wheels[0] == null || wheels[1] == null) { throw new CarMissingComponentException("FrontWheels"); }

            wheels[0].ApplySteer(steeringInput);
            wheels[1].ApplySteer(steeringInput);
        }
        catch (CarMissingComponentException exception)
        {
            Debug.LogWarning(exception.Message + exception.GetMissingComponent);
        }

        return;
    }

    // Crude traction control that reduces the power to wheel if the car is wheel spinning too much
    protected void TractionControl()
    {
        try
        {
            if (engine == null) { throw new CarMissingComponentException("TheEngine"); }
            if (wheels == null) { throw new CarMissingComponentException("AlTheWheels"); }

            foreach (Wheel wheelToCheck in wheels)
            {
                float currentSlip = wheelToCheck.GetCurrentSlip;
                engine.AdjustTorque(currentSlip);
            }
        }
        catch (CarMissingComponentException exception)
        {
            Debug.LogWarning(exception.Message + exception.GetMissingComponent);
        }

        return;
    }

    protected override void Update()
    {
        HandleInput();

        base.Update();

        return;
    }
}

public class CarMissingComponentException : Exception
{
    protected string missingComponent;
    public string GetMissingComponent { get { return missingComponent; } }

    public CarMissingComponentException() : base("The car is missing a component: ")
    {

    }

    public CarMissingComponentException(string _component) : base("The car is missing a component: ")
    {
        missingComponent = _component;

        return;
    }
}
