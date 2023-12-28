using CBA.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEmitterModule : Module
{
    [SerializeField] private AudioEmitter _audioEmitter;

    public void PlaySfx(string audioName)
    {
        _audioEmitter?.PlayOneShotSfx(audioName);
    }

    /// <summary>
    /// Unparent the emitter, such as in cases that the attached entity is dead / disabled.
    /// </summary>
    public void UnparentAndDelayedDestroyEmitter(float destroyDelay = 3f)
    {
        _audioEmitter.transform.SetParent(null);
        Destroy(_audioEmitter.gameObject, destroyDelay);
    }
}
