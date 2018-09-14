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

    public float GetFootBrakeTorqueToApply(float _footBrakeInput)
    {
        if(_footBrakeInput < 0) {
            _footBrakeInput = 0;
        }
        float footBrakeTorqueToApply = 0;
        footBrakeTorqueToApply = maxReverseTorque * _footBrakeInput;
        return footBrakeTorqueToApply;
    }

    public float GetHandBrakeForceToApply(float _handBrakeInput)
    {
        if (_handBrakeInput < 0) {
            _handBrakeInput = 0;
        }
        float handBrakeTorqueToApply = 0;
        handBrakeTorqueToApply = maxHandBrakeTorque * _handBrakeInput;
        return handBrakeTorqueToApply;
    }
}
