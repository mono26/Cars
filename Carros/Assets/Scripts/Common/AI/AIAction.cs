using UnityEngine;

public abstract class AIAction : ScriptableObject
{
    public abstract void DoAction(Entity _entity);
}
