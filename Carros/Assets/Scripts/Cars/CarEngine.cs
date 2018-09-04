using UnityEngine;

public class CarEngine : EntityComponent
{
    [Header("Engine settings")]
    [SerializeField] protected float maxSpeed = 150; // In M/H (Milles per hour).
    [SerializeField] protected float maxTorqueOverTheWheels = 2500f;
    [SerializeField] protected float maxWheelSlip = 0.3f;
    [SerializeField] [Range(0,1)] protected float revolutionsRangeBoundary = 1.0f;
    [SerializeField] protected float tractionControl = 1.0f;

    [Header("Editor debugging")]
    [SerializeField] protected float currentTorque;
    [SerializeField] protected float currentRevolutions;
    
    /// <summary>
    /// Get the current speed in MPH.
    /// </summary>
    public float GetCurrentSpeed { get { return entity.GetBody.velocity.magnitude * 2.23693629f; } }

    /// <summary>
    /// Used to adjust the torque based on the slip factor.
    /// </summary>
    /// <param name="_forwardSlip"> Usually is the slip of a wheel.</param>
    public void AdjustTorque(float _forwardSlip)
    {
        float newTorque = 0;
        if (_forwardSlip >= maxWheelSlip && currentTorque >= 0) { newTorque = currentTorque - (10 * tractionControl); }
        else
        {
            newTorque = currentTorque + (10 * tractionControl);
            if (newTorque > maxTorqueOverTheWheels) { newTorque = maxTorqueOverTheWheels; }
        }

        currentTorque = newTorque;

        return;
    }

    /// <summary>
    /// Apply the drive input to the engine.
    /// </summary>
    /// <param name="_accelerationInput"> The acceleration input, usually from 0 to 1</param>
    /// <param name="_footBrakeInput">The footbrake input, usually from -1 to 0 </param>
    public float GetTorqueToApply(float _accelerationInput)
    {
        float thrustTorque = 0;
        thrustTorque = _accelerationInput * (currentTorque / 4f);

        return thrustTorque;
    }

    private void CalculateRevs()
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        float revsFactor = GetCurrentSpeed / maxSpeed;
        currentRevolutions = Mathf.Lerp(0, 1, revsFactor);

        return;
    }

    protected void CapSpeed()
    {
        Rigidbody carBody = entity.GetBody;     
        float speed = carBody.velocity.magnitude;
        speed *= 2.23693629f;
        if (speed > maxSpeed)
            carBody.velocity = (maxSpeed / 2.23693629f) * carBody.velocity.normalized;

        return;
    }

    public override void FixedFrame()
    {
        CapSpeed();
        CalculateRevs();

        return;
    }

    protected void Start()
    {
        currentTorque = maxTorqueOverTheWheels - (tractionControl * maxTorqueOverTheWheels);

        return;
    }
}
