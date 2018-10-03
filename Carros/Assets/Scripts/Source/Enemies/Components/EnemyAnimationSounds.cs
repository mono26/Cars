// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public class EnemyAnimationSounds : MonoBehaviour
{
    [Header("Enemy Animation Sounds settings")]
    [SerializeField]
    protected AudioClip backStep;
    [SerializeField]
    protected AudioClip frontStep;
    [SerializeField]
    protected AudioClip idleAudio;

    public void Grunt()
    {
        /*if (gruntAudio != null)
            gruntAudio.PlayRandomClip();*/

        return;
    }

    protected void PlayStep(int frontFoot)
    {
        /*if (frontStepAudio != null && frontFoot == 1)
            frontStepAudio.PlayRandomClip();
        else if (backStepAudio != null && frontFoot == 0)
            backStepAudio.PlayRandomClip();*/

        return;
    }
}
