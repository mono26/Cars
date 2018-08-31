using UnityEngine;

public class Wheel : CarComponent
{
    public enum TorqueType { Accelerate, Break}

    [Header("Wheel settings")]
    [SerializeField]
    protected float downForce;
    [SerializeField]
    protected float maxTorque;
    [SerializeField]
    protected float maxWheelSlip;
    [SerializeField]
    protected float tractionControl;
    [SerializeField]
    protected WheelCollider wheelCollider;

    // This is used to add more grip in relation to speed
    protected void AddDownForce()
    {
        Rigidbody wheelBody = wheelCollider.attachedRigidbody;
        wheelBody.AddForce(-transform.up * downForce * wheelBody.velocity.magnitude);

        return;
    }

    protected void AdjustTorque(float _forwardSlip)
    {
        float newTorque = 0;
        float currentTorque = wheelCollider.motorTorque;
        if (_forwardSlip >= maxWheelSlip && currentTorque >= 0)
        {
            newTorque = currentTorque - (10 * tractionControl);
        }
        else
        {
            newTorque = currentTorque + (10 * tractionControl);
            if (newTorque > maxTorque)
            {
                newTorque = maxTorque;
            }
        }

        return;
    }

    public void ApplyTorque(float _torqueToApply = 0.0f, TorqueType _typeOfTorque = TorqueType.Accelerate)
    {
        if (_typeOfTorque == TorqueType.Accelerate) { wheelCollider.motorTorque = _torqueToApply; }
        else { wheelCollider.brakeTorque = _torqueToApply; }

        return;
    }

    protected override void Awake()
    {
        if(wheelCollider == null)
            wheelCollider = GetComponent<WheelCollider>();

        return;
    }

    // Checks if the wheels are spinning and is so does three things
    // 1) emits particles
    // 2) plays tiure skidding sounds
    // 3) leaves skidmarks on the ground
    // these effects are controlled through the WheelEffects class
    protected void CheckForWheelSpin()
    {
        // loop through all wheels
        for (int i = 0; i < 4; i++)
        {
            WheelHit wheelHit;
            wheelCollider.GetGroundHit(out wheelHit);

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

    public override void FixedFrame()
    {
        SteerHelp();
        AddDownForce();
        CheckForWheelSpin();
        TractionControl();

        return;
    }

    public void SetSteerAngle(float _targetAngle)
    {
        wheelCollider.steerAngle = _targetAngle;

        return;
    }

    protected void SteerHelp()
    {
        WheelHit wheelhit;
        wheelCollider.GetGroundHit(out wheelhit);
        if (wheelhit.normal == Vector3.zero)
            return; // wheels arent on the ground so dont realign the rigidbody velocity
    }

    // Crude traction control that reduces the power to wheel if the car is wheel spinning too much
    protected void TractionControl()
    {
        WheelHit wheelHit;
        wheelCollider.GetGroundHit(out wheelHit);
        AdjustTorque(wheelHit.forwardSlip);

        return;
    }
}
