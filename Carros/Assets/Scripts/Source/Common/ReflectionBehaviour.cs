// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionBehaviour : MonoBehaviour
{
    public bool AwakeCalled { get; private set; }
    public bool StartCalled { get; private set; }
    public bool OnEnableCalled { get; private set; }
    public bool CustomCalled { get; private set; }

    protected void Awake()
    {
        AwakeCalled = true;
        return;
    }

    protected void Start()
    {
        StartCalled = true;
        return;
    }

    protected void OnEnable()
    {
        OnEnableCalled = true;
        return;
    }

    protected void CustomMethod()
    {
        CustomCalled = true;
        return;
    }
}
