using System;
using UnityEngine;

[RequireComponent(typeof(CarEngine), typeof(Brakes))]
public class Car : Entity
{
    [Header("Car settings")]
    [SerializeField] private Vector3 m_CentreOfMassOffset = Vector3.zero;
    [SerializeField] [Range(0, 1)] private float directionAssist = 0.644f; // 0 is raw physics , 1 the car will grip in the direction it is facing.

    [Header("Car components")]
    [SerializeField] private Brakes brakes;
    [SerializeField] private CarEngine engine;
    [SerializeField] private Wheel[] wheels;  // Assuming 0 and 1 are the front wheels and 2 and 3 the back wheels.

    [Header("Editor debugging")]
    [SerializeField] private float accelerationInput;
    [SerializeField] private float footBrakeInput;
    [SerializeField] private float handBrakeInput;
    [SerializeField] private float oldRotation;
    [SerializeField] private float steeringInput;

    public CarEngine GetEngine { get { return engine; } }
    public Wheel[] GetWheels { get { return wheels; } }

    protected override void Awake()
    {
        if (brakes == null) {
            brakes = GetComponent<Brakes>();
        }
        if (engine == null) {
            engine = GetComponent<CarEngine>();
        }
        if (wheels == null) {
            wheels = GetComponentsInChildren<Wheel>();
        }
        base.Awake();
        return;
    }

    private void Start()
    {
        /*m_WheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++)
        {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }
        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;*/
    }

    protected override void FixedUpdate()
    {
        DirectionAsssist();
        Drive();
        CapCarSpeed();
        TractionControl();
        base.FixedUpdate();
        return;
    }

    private void DirectionAsssist()
    {
        if (HasAllWheelsGrounded())
        {
            // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            if (Mathf.Abs(oldRotation - transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (transform.eulerAngles.y - oldRotation) * directionAssist;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                SetBodyVelocity(velRotation * GetBody.velocity);
            }
            oldRotation = transform.eulerAngles.y;
        }
        return;
    }

    private bool HasAllWheelsGrounded()
    {
        bool hasAllWheelsGrounded = false;
        if (HasAllWheels())
        {
            foreach (Wheel wheelToCheck in wheels)
            {
                if (!wheelToCheck.WheelIsGrounded())
                {
                    hasAllWheelsGrounded = false;
                    break;
                }
            }
        }
        return hasAllWheelsGrounded;
    }

    private bool HasAllWheels()
    {
        bool hasAllWheels = false;
        if (HasBackWheels() || HasFrontWheels())
        {
            hasAllWheels = true;
        }
        return hasAllWheels;
    }

    private bool HasBackWheels()
    {
        bool hasBackWheels = true;
        try
        {
            if (wheels == null)
            {
                hasBackWheels = false;
                throw new MissingComponentException("The car has a all missing  wheels: ");
            }
            else
            {
                int numberOfWheels = wheels.Length;
                // We start at the half number of wheels to exclude the front.
                for (int i = numberOfWheels / 2; i < numberOfWheels; i++)
                {
                    if (wheels[i] == null)
                    {
                        hasBackWheels = false;
                        throw new MissingComponentException("The car has a missing rear wheel: ", typeof(Wheel));
                    }
                }
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasBackWheels;
    }

    private bool HasFrontWheels()
    {
        bool hasFrontWheels = true;
        try
        {
            if (wheels == null)
            {
                hasFrontWheels = false;
                throw new MissingComponentException("The car has a missing  wheels: ");
            }
            else
            {
                int numberOfFrontWheels = wheels.Length / 2;
                for (int i = 0; i < numberOfFrontWheels; i++)
                {
                    if (wheels[i] == null)
                    {
                        hasFrontWheels = false;
                        throw new MissingComponentException("The car has a missing front wheel: ", typeof(Wheel));
                    }
                }
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasFrontWheels;
    }

    private void Drive()
    {
        SteerFrontWheels();
        ApplyEngineTorqueToAllWheels();
        ApplyBrakes();
        return;
    }

    private void SteerFrontWheels()
    {
        if (HasFrontWheels())
        {
            wheels[0].ApplySteer(steeringInput);
            wheels[1].ApplySteer(steeringInput);
        }
        return;
    }

    private void ApplyEngineTorqueToAllWheels()
    {
        if (HasEngine() && HasAllWheels())
        {
            float accelerationTorque = engine.GetCarEngineTorqueToApply(accelerationInput) / wheels.Length;
            foreach (Wheel wheel in wheels)
            {
                wheel.SetTorque(accelerationTorque, Wheel.TorqueType.Acceleration);
            }
        }
        return;
    }

    private bool HasEngine()
    {
        bool hasEngine = true;
        try
        {
            if (engine == null)
            {
                hasEngine = false;
                throw new MissingComponentException("The car has a missing engine: ", typeof(CarEngine));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasEngine;
    }

    private void ApplyBrakes()
    {
        HandBrake();
        FootBrake();
        return;
    }

    private void HandBrake()
    {
        if (HasFrontWheels() && HasBrakes())
        {
            if (handBrakeInput > 0)
            {
                float handBrakeTorque = brakes.GetHandBrakeForceToApply(handBrakeInput);
                wheels[0].SetTorque(handBrakeTorque, Wheel.TorqueType.Braking);
                wheels[1].SetTorque(handBrakeTorque, Wheel.TorqueType.Braking);
            }
        }
        return;
    }

    private bool HasBrakes()
    {
        bool hasBrakes = true;
        try
        {
            if (brakes == null)
            {
                hasBrakes = false;
                throw new MissingComponentException("The car has missing brakes: ", typeof(Brakes));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasBrakes;
    }

    private void FootBrake()
    {
        if (HasBrakes() && HasAllWheels())
        {
            if (footBrakeInput > 0)
            {
                float footBrakeTorque = -brakes.GetFootBrakeTorqueToApply(footBrakeInput);
                foreach (Wheel wheel in wheels)
                {
                    wheel.SetTorque(footBrakeTorque, Wheel.TorqueType.Acceleration);
                }
            }
        }
        return;
    }

    private void CapCarSpeed()
    {
        Vector3 actualVelocity = GetBody.velocity;
        Vector3 clampedVelocity = engine.CapVelocityMagnitudeToMaxSpeed(actualVelocity);
        SetBodyVelocity(clampedVelocity);
        return;
    }

    private void CalculetCarRevolutions()
    {
        float currentSpeed = GetBody.velocity.magnitude;
        engine.CalculateEngineRevolutions(currentSpeed);
        return;
    }

    // Crude traction control that reduces the power to wheel if the car is wheel spinning too much
    private void TractionControl()
    {
        if (HasEngine() && HasAllWheels())
        {
            foreach (Wheel wheelToCheck in wheels)
            {
                float currentSlip = wheelToCheck.GetWheelSlip();
                engine.AdjustTorque(currentSlip);
            }
        }
        return;
    }

    protected override void Update()
    {
        HandleInput();
        base.Update();
        return;
    }

    private void HandleInput()
    {
        if (!CanApplyExternalInputToEntity()) { return; }
        HandleAccelerationInput();
        HandleBrakesInput();
        HandleSteeringInput();
        return;
    }

    private void HandleAccelerationInput()
    {
        if (CanApplyExternalInputToEntity())
        {
            accelerationInput = GetInputcomponent.GetMovementInput;
            accelerationInput = Mathf.Clamp(accelerationInput, 0, 1);
        }
        return;
    }

    private void HandleBrakesInput()
    {
        if (!CanApplyExternalInputToEntity())
        {
            // TODO refactor input access.
            footBrakeInput = GetInputcomponent.GetFootBrakesInput;
            footBrakeInput = -1 * Mathf.Clamp(footBrakeInput, -1, 0);
            handBrakeInput = GetInputcomponent.GetHandBrakeInput;
            handBrakeInput = Mathf.Clamp(handBrakeInput, 0, 1);
        }
        return;
    }

    private void HandleSteeringInput()
    {
        if (!CanApplyExternalInputToEntity())
        {
            steeringInput = GetInputcomponent.GetSteeringInput;
            steeringInput = Mathf.Clamp(steeringInput, -1, 1);
        }
        return;
    }

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
}
