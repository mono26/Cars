// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

[System.Serializable]
public class CarInput : EntityInput
{
    [SerializeField] private float accelerationInput = 0;
    [SerializeField] private float footBrakeInput = 0;
    [SerializeField] private float handBrakeInput = 0;
    [SerializeField] private float steeringInput = 0;

    public float GetAccelerationInput { get { return accelerationInput; } }
    public float GetFootBrakeInput { get { return footBrakeInput; } }
    public float GetHandBrakeInput { get { return handBrakeInput; } }
    public float GetSteeringInput { get { return steeringInput; } }

    public CarInput(float _accelerationInput, float _footBrakeInput, float _handBrakeInput, float _steeringInput)
    {
        accelerationInput = Mathf.Clamp(_accelerationInput, 0, 1);
        footBrakeInput = -1 * Mathf.Clamp(_footBrakeInput, -1, 0);
        handBrakeInput = Mathf.Clamp(_handBrakeInput, 0, 1);
        steeringInput = Mathf.Clamp(_steeringInput, -1, 1);
        return;
    }
}

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

    [Header("Car editor debugging")]
    [SerializeField] private float oldRotation;
    [SerializeField] private CarInput currentInput;

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
        bool hasAllWheelsGrounded = true;
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
        if (HasBackWheels() && HasFrontWheels())
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
                        throw new MissingComponentException(gameObject, typeof(Wheel));
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
                        throw new MissingComponentException(gameObject, typeof(Wheel));
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
            wheels[0].ApplySteer(currentInput.GetSteeringInput);
            wheels[1].ApplySteer(currentInput.GetSteeringInput);
        }
        return;
    }

    private void ApplyEngineTorqueToAllWheels()
    {
        if (HasEngine() && HasAllWheels())
        {
            float accelerationTorque = engine.GetCarEngineTorqueToApply(currentInput.GetAccelerationInput) / wheels.Length;
            foreach (Wheel wheel in wheels) {
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
                throw new MissingComponentException(gameObject, typeof(CarEngine));
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
            float handBrakeTorque = brakes.GetHandBrakeForceToApply(currentInput.GetHandBrakeInput);
            wheels[0].SetTorque(handBrakeTorque, Wheel.TorqueType.Braking);
            wheels[1].SetTorque(handBrakeTorque, Wheel.TorqueType.Braking);
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
                throw new MissingComponentException(gameObject, typeof(Brakes));
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
            if (currentInput.GetFootBrakeInput > 0)
            {
                float footBrakeTorque = -brakes.GetFootBrakeTorqueToApply(currentInput.GetFootBrakeInput);
                foreach (Wheel wheel in wheels) {
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
        base.Update();
        return;
    }

    public override void ReceiveInput(EntityInput _inputToRecieve)
    {
        if (_inputToRecieve is CarInput) {
            currentInput = _inputToRecieve as CarInput;
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
