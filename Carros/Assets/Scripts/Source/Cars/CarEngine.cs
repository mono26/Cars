using UnityEngine;

public class CarEngine : EntityComponent
{
    [Header("Engine settings")]
    [SerializeField] private float maxSpeed = 150; // In M/H (Milles per hour).
    [SerializeField] private float maxTorque = 2500f;
    [SerializeField] private float maxWheelSlip = 0.3f;
    [SerializeField] private float tractionControl = 1.0f;

    [Header("Editor debugging")]
    [SerializeField] private float currentTorque;

    private void Start()
    {
        currentTorque = maxTorque - (tractionControl * maxTorque);
        return;
    }

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
            if (newTorque > maxTorque) { newTorque = maxTorque; }
        }
        currentTorque = newTorque;
        return;
    }

    /// <summary>
    /// Apply the drive input to the engine.
    /// </summary>
    /// <param name="_accelerationInput"> The acceleration input, usually from 0 to 1</param>
    /// <param name="_footBrakeInput">The footbrake input, usually from -1 to 0 </param>
    public float GetCarEngineTorqueToApply(float _accelerationInput)
    {
        // Only generates positive torque.
        if (_accelerationInput < 0) {
            _accelerationInput = 0;
        }
        float thrustTorque = 0;
        thrustTorque = _accelerationInput * currentTorque;
        return thrustTorque;
    }

    public Vector3 CapVelocityMagnitudeToMaxSpeed(Vector3 _velocityToCap)
    {
        float speed = _velocityToCap.magnitude;
        Vector3 capedVelocity = _velocityToCap;
        speed *= 2.23693629f;
        if (speed > maxSpeed)
        {
            capedVelocity = (maxSpeed / 2.23693629f) * _velocityToCap.normalized;
        }
        return capedVelocity;
    }

    public float CalculateEngineRevolutions(float _currentSpeed)
    {
        // calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        float revsFactor = _currentSpeed / maxSpeed;
        float currentRevolutions = Mathf.Lerp(0, 1, revsFactor);
        return currentRevolutions;
    }
}
