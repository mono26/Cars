// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

[RequireComponent(typeof(WheelCollider))]
public class Wheel : EntityComponent
{
    public enum TorqueType { Acceleration, Braking }
    public enum WheelType { Rear, Front }

    [Header("Wheel settings")]
    [SerializeField] private float downForce = 100; // Helps the car stick to the ground.
    [SerializeField] private float maxSteerAngle = 25;
    [SerializeField] private WheelCollider wheelColliderComponent;
    [SerializeField] private WheelType wheelType = WheelType.Front;
    //[SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];

    protected override void Awake()
    {
        if (wheelColliderComponent == null) {
            wheelColliderComponent = GetComponent<WheelCollider>();
        }
        return;
    }

    public override void FixedFrame()
    {
        AddDownForce();
        CheckForWheelSpin();
        return;
    }

    // This is used to add more grip in relation to speed
    private void AddDownForce()
    {
        try
        {
            if (HasWheelColliderComponent())
            {
                Rigidbody wheelBody = wheelColliderComponent.attachedRigidbody;
                wheelBody.AddForce(-transform.up * downForce * wheelBody.velocity.magnitude);
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return;
    }

    private bool HasWheelColliderComponent()
    {
        bool hasWheelCollider = true;
        try
        {
            if (wheelColliderComponent == null)
            {
                hasWheelCollider = false;
                throw new MissingComponentException(gameObject, typeof(WheelCollider));
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return hasWheelCollider;
    }

    // Checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    private void CheckForWheelSpin()
    {
        // loop through all wheels
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            wheelColliderComponent.GetGroundHit(out wheelHit);

            // is the tire slipping above the given threshhold
            /*if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
            {
                m_WheelEffects[i].EmitTyreSmoke();

                // avoiding all four tires screeching at the same time
                // if they do it can lead to some strange audio artefacts
                if (!AnySkidSoundPlaying())
                {
                    m_WheelEffects[i].PlayAudio();
                }
                continue;
            }

            // if it wasnt slipping stop all the audio
            if (m_WheelEffects[i].PlayingAudio)
            {
                m_WheelEffects[i].StopAudio();
            }
            // end the trail generation
            m_WheelEffects[i].EndSkidTrail();*/
        }

        return;
    }

    public void SetTorque(float _torqueToApply = 0.0f, TorqueType _typeOfTorque = TorqueType.Acceleration)
    {
        if (HasWheelColliderComponent())
        {
            if (_typeOfTorque == TorqueType.Acceleration) {
                wheelColliderComponent.motorTorque = _torqueToApply;
            }
            else if (_typeOfTorque == TorqueType.Braking) {
                wheelColliderComponent.brakeTorque = _torqueToApply;
            }
        }
        return;
    }

    public void ApplySteer(float _steerInput)
    {
        if (HasWheelColliderComponent())
        {
            float steerAngleToApply = _steerInput * maxSteerAngle;
            wheelColliderComponent.steerAngle = steerAngleToApply;
        }
        return;
    }

    public float GetWheelSlip()
    {
        float wheelSlip = 0;
        try
        {
            if (HasWheelColliderComponent())
            {
                WheelHit wheelHit;
                wheelColliderComponent.GetGroundHit(out wheelHit);
                wheelSlip = wheelHit.forwardSlip;
            }
        }
        catch (MissingComponentException missingComponentException) {
            missingComponentException.DisplayException();
        }
        return wheelSlip;
    }

    public bool WheelIsGrounded()
    {
        bool isGrounded = false;
        if (HasWheelColliderComponent())
        {
            WheelHit wheelhit;
            wheelColliderComponent.GetGroundHit(out wheelhit);
            if (wheelhit.normal == Vector3.zero) {
                isGrounded = false;
            }
        }
        return isGrounded;
    }
}
