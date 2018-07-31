using UnityEngine;

public abstract class AIDecision : ScriptableObject
{
    public abstract bool Decide(Entity _entity);
}
