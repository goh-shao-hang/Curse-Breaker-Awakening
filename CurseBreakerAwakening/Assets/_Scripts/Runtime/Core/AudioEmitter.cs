using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CBA.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEmitter : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.SfxMixerGroup;
            _audioSource.spatialBlend = 1f;
        }

        public void PlayOneShotSfx(string audioName)
        {
            if (!AudioManager.Instance.SfxExist(audioName))
            {
                Debug.LogWarning($"Sound effect {audioName} not found!");
                return;
            }

            Audio soundEffect = AudioManager.Instance?.GetSoundEffect(audioName);

            _audioSource.volume = soundEffect.Volume;
            _audioSource.pitch = soundEffect.Pitch + Random.Range(-soundEffect.PitchVariation * 0.5f, soundEffect.PitchVariation * 0.5f);

            _audioSource.PlayOneShot(soundEffect.GetClip());
        }
    }
}