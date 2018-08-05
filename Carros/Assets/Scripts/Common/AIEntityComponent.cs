using UnityEngine;

public class AIEntityComponent : EntityComponent
{
    [Header("AIEntity Component settings")]
    [SerializeField]
    protected Enemy aiEntity;

    protected override void Awake()
    {
        base.Awake();

        if(aiEntity == null)
            aiEntity = entity as Enemy;

        return;
    }
}
