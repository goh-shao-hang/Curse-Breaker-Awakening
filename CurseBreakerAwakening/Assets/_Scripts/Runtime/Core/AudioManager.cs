using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CBA.Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private SO_AudioDatabase _database;
        [SerializeField] private AudioSource _globalBGMAudioSource;
        [SerializeField] private AudioSource _globalSFXAudioSource;
        [field: SerializeField] public AudioMixerGroup BgmMixerGroup { get; private set; }
        [field: SerializeField] public AudioMixerGroup SfxMixerGroup { get; private set; }


        private Dictionary<string, Audio> _musicDictionary = new Dictionary<string, Audio>();
        private Dictionary<string, Audio> _soundEffectsDictionary = new Dictionary<string, Audio>();

        protected override void Awake()
        {
            base.Awake();

            this.SetDontDestroyOnLoad(true);

            foreach (Audio audio in _database.Musics)
            {
                if (!_musicDictionary.ContainsKey(audio.Name))
                {
                    _musicDictionary.Add(audio.Name, audio);
                }
                else
                {
                    Debug.LogWarning($"The name of music '{audio.Name}' is repeated. Only first occurence is added.");
                }
            }

            foreach (Audio audio in _database.SoundEffects)
            {
                if (!_soundEffectsDictionary.ContainsKey(audio.Name))
                {
                    _soundEffectsDictionary.Add(audio.Name, audio);
                }
                else
                {
                    Debug.LogWarning($"The name of sound effect '{audio.Name}' is repeated. Only first occurence is added.");
                }
            }
        }

        public void PlayBGM(string audioName)
        {
            if (_musicDictionary.ContainsKey(audioName))
            {
                Audio audio = _musicDictionary[audioName];

                _globalBGMAudioSource.clip = audio.Play();
                _globalBGMAudioSource.volume = audio.Volume;
                _globalBGMAudioSource.pitch = audio.Pitch + Random.Range(-audio.PitchVariation * 0.5f, audio.PitchVariation * 0.5f);

                _globalBGMAudioSource.Play();
            }
            else
            {
                Debug.LogError($"Music of name {audioName} not found!");
            }
        }

        public async void StopBGM(float fadeOutTime = 0f)
        {
            if (!_globalBGMAudioSource.isPlaying)
                return;

            if (fadeOutTime > 0f) 
            {
                await DOVirtual.Float(_globalBGMAudioSource.volume, 0, fadeOutTime, (volume) => _globalBGMAudioSource.volume = volume).AsyncWaitForCompletion();
            }

            _globalBGMAudioSource.Stop();
        }

        public void PlayGlobalSFX(string audioName)
        {
            if (_soundEffectsDictionary.ContainsKey(audioName))
            {
                Audio audio = _soundEffectsDictionary[audioName];

                _globalSFXAudioSource.volume = audio.Volume;
                _globalSFXAudioSource.pitch = audio.Pitch + Random.Range(-audio.PitchVariation * 0.5f, audio.PitchVariation * 0.5f);

                _globalSFXAudioSource.PlayOneShot(audio.Play());
            }
            else
            {
                Debug.LogError($"Sound Effect of name {audioName} not found!");
            }
        }

        public Audio GetSoundEffect(string audioName) //Retrieve audio to play locally at specific position
        {
            if (_soundEffectsDictionary.ContainsKey(audioName))
            {
                return _soundEffectsDictionary[audioName];
            }
            else
            {
                Debug.LogError($"Sound Effect of name {audioName} not found!");
                return null;
            }
        }

        public bool MusicExist(string audioName)
        {
            return _musicDictionary.ContainsKey(audioName);
        }

        public bool SfxExist(string audioName)
        {
            return _soundEffectsDictionary.ContainsKey(audioName);
        }
    }
}