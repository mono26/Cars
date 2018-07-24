using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarComponent : MonoBehaviour
{
    [SerializeField]
    protected Car car;

    protected virtual void Awake()
    {
        if (car == null)
            car = GetComponent<Car>();

        return;
    }

    public virtual void EveryFrame()
    {
        HandleInput();

        return;
    }

    public virtual void FixedFrame()
    {

    }

    public virtual void LateFrame()
    {

    }

    protected virtual void HandleInput()
    {

    }
}
