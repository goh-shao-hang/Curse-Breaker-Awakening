using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CBA.Core
{
    [Serializable]
    public class Audio
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public AudioClip[] AudioClips { get; private set; }
        [Range(0, 1)] [SerializeField] private float _volume = 1f;
        [Range(-3, 3)][SerializeField] private float _pitch = 1f;
        [Range(0, 1)][SerializeField] private float _pitchVariation = 0f;

        public float Volume => _volume;
        public float Pitch => _pitch;
        public float PitchVariation => _pitchVariation;

        public AudioClip GetClip()
        {
            int random = Random.Range(0, AudioClips.Length);
            return AudioClips[random];
        }
    }
}