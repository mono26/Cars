using UnityEngine;

public class Wheel : EntityComponent
{
    public enum TorqueType { Acceleration, Braking }
    public enum WheelType { Rear, Front }

    [Header("Wheel settings")]
    [SerializeField] private float downForce = 100; // Helps the car stick to the ground.
    [SerializeField] private float maxSteerAngle = 25;
    [SerializeField] private WheelCollider wheelCollider;
    [SerializeField] private WheelType wheelType = WheelType.Front;
    //[SerializeField] private WheelEffects[] m_WheelEffects = new WheelEffects[4];

    public WheelCollider GetWheelCollider { get { { return wheelCollider; } } }
    public float GetCurrentSlip
    {
        get
        {
            WheelHit wheelHit;
            wheelCollider.GetGroundHit(out wheelHit);
            return wheelHit.forwardSlip;
        }
    }

    protected override void Awake()
    {
        if (wheelCollider == null)
            wheelCollider = GetComponent<WheelCollider>();
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
        if (wheelCollider == null) { return; }
        Rigidbody wheelBody = wheelCollider.attachedRigidbody;
        wheelBody.AddForce(-transform.up * downForce * wheelBody.velocity.magnitude);
        return;
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

    public void SetTorque(float _torqueToApply = 0.0f, TorqueType _typeOfTorque = TorqueType.Acceleration)
    {
        if (wheelCollider == null) { return; }
        if (_typeOfTorque == TorqueType.Acceleration) { wheelCollider.motorTorque = _torqueToApply; }
        else if (_typeOfTorque == TorqueType.Braking) { wheelCollider.brakeTorque = _torqueToApply; }
        return;
    }

    public void ApplySteer(float _steerInput)
    {
        if (wheelCollider == null) { return; }
        float steerAngleToApply = _steerInput * maxSteerAngle;
        wheelCollider.steerAngle = steerAngleToApply;
        return;
    }
}
