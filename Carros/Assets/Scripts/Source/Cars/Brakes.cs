using UnityEngine;

public class Brakes : EntityComponent
{
    [Header("Brakes settings")]
    [SerializeField] private float maxHandBrakeTorque = 1e+08f;
    [SerializeField] private float maxReverseTorque = 500;

    protected void Start()
    {
        maxHandBrakeTorque = float.MaxValue;
        return;
    }

    public float GetFootBrakeForceToApply(float _footBrakeInput)
    {
        float footBrakeTorqueToApply = 0;
        footBrakeTorqueToApply = maxReverseTorque * _footBrakeInput;
        return footBrakeTorqueToApply;
    }

    public float GetHandBrakeForceToApply(float _handBrakeInput)
    {
        float handBrakeTorqueToApply = 0;
        handBrakeTorqueToApply = maxHandBrakeTorque * _handBrakeInput;
        return handBrakeTorqueToApply;
    }
}
