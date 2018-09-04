using UnityEngine;

public class Brakes : EntityComponent
{
    [Header("Brakes settings")]
    [SerializeField] protected float maxHandBrakeTorque = 1e+08f;
    [SerializeField] protected float maxReverseTorque = 500;

    public float GetFootBrakeForceToApply(float _footBrakeInput)
    {
        float footBrakeTorqueToApply = 0;
        if (_footBrakeInput > 0)
        {
            footBrakeTorqueToApply = maxReverseTorque * _footBrakeInput;
        }

        return footBrakeTorqueToApply;
    }

    public float GetHandBrakeForceToApply(float _handBrakeInput)
    {
        float handBrakeTorqueToApply = 0;
        if (_handBrakeInput > 0)
        {
            handBrakeTorqueToApply = maxHandBrakeTorque * _handBrakeInput;
        }

        return handBrakeTorqueToApply;
    }

    protected void Start()
    {
        maxHandBrakeTorque = float.MaxValue;

        return;
    }
}
