using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : CarComponent
{
    [Header("Engine settings")]
    [SerializeField]
    protected float maxSpeed; // In Km/H.

    private void ApplyDrive(float accel, float footbrake)
    {
        /*float thrustTorque;
        switch (m_CarDriveType)
        {
            case CarDriveType.FourWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 4f);
                for (int i = 0; i < 4; i++)
                {
                    m_WheelColliders[i].motorTorque = thrustTorque;
                }
                break;

            case CarDriveType.FrontWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 2f);
                m_WheelColliders[0].motorTorque = m_WheelColliders[1].motorTorque = thrustTorque;
                break;

            case CarDriveType.RearWheelDrive:
                thrustTorque = accel * (m_CurrentTorque / 2f);
                m_WheelColliders[2].motorTorque = m_WheelColliders[3].motorTorque = thrustTorque;
                break;

        }

        for (int i = 0; i < 4; i++)
        {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
            {
                m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
            }
        }*/
    }

    private void CalculateRevs()
    {
        /*// calculate engine revs (for display / sound)
        // (this is done in retrospect - revs are not used in force/power calculations)
        CalculateGearFactor();
        var gearNumFactor = m_GearNum / (float)NoOfGears;
        var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
        var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
        Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);*/

        return;
    }

    private void CapSpeed()
    {
        Rigidbody carBody = car.Body;     
        float speed = carBody.velocity.magnitude;
        speed *= 3.6f;
        if (speed > maxSpeed)
            carBody.velocity = (maxSpeed / 3.6f) * carBody.velocity.normalized;

        return;
    }
}
