// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public abstract class AIDecision : ScriptableObject
{
    public abstract bool Decide(Entity _entity);
}
