using DG.Tweening;
using GameCells.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace CBA.Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private SO_AudioDatabase _database;
        [SerializeField] private AudioSource _globalBGMAudioSource1;
        [SerializeField] private AudioSource _globalBGMAudioSource2;
        [SerializeField] private AudioSource _globalSFXAudioSource;
        [field: SerializeField] public AudioMixerGroup BgmMixerGroup { get; private set; }
        [field: SerializeField] public AudioMixerGroup SfxMixerGroup { get; private set; }


        private Dictionary<string, Audio> _musicDictionary = new Dictionary<string, Audio>();
        private Dictionary<string, Audio> _soundEffectsDictionary = new Dictionary<string, Audio>();

        private Dictionary<string, float> _musicTimeDictionary = new Dictionary<string, float>();

        private Audio _currentBGM;

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
                _globalBGMAudioSource2.Stop();

                Audio audio = _musicDictionary[audioName];

                _globalBGMAudioSource1.clip = audio.GetClip();
                _globalBGMAudioSource1.volume = audio.Volume;
                _globalBGMAudioSource1.pitch = audio.Pitch + Random.Range(-audio.PitchVariation * 0.5f, audio.PitchVariation * 0.5f);

                _globalBGMAudioSource1.time = 0f;

                _globalBGMAudioSource1.Play();

                _currentBGM = audio;
            }
            else
            {
                Debug.LogError($"Music of name {audioName} not found!");
            }
        }

        public void CrossFadeBGM(string audioName, float duration = 2f, bool continueIfPlayedBefore = false)
        {
            if (!_musicDictionary.ContainsKey(audioName))
            {
                Debug.LogError($"Music of name {audioName} not found!");
                return;
            }

            Audio music = _musicDictionary[audioName];

            _globalBGMAudioSource1.DOKill(true);
            _globalBGMAudioSource2.DOKill(true);

            if (_globalBGMAudioSource1.isPlaying)
            {
                if (_currentBGM != null)
                {
                    if (!_musicTimeDictionary.ContainsKey(_currentBGM.Name))
                    {
                        _musicTimeDictionary.Add(_currentBGM.Name, _globalBGMAudioSource1.time);
                    }
                    else
                    {
                        _musicTimeDictionary[_currentBGM.Name] = _globalBGMAudioSource1.time;
                    }
                }

                _globalBGMAudioSource2.clip = music.GetClip();
                _globalBGMAudioSource2.pitch = music.Pitch + Random.Range(-music.PitchVariation * 0.5f, music.PitchVariation * 0.5f);
                _globalBGMAudioSource2.volume = 0f;

                if (continueIfPlayedBefore && _musicTimeDictionary.ContainsKey(audioName))
                {
                    _globalBGMAudioSource2.time = _musicTimeDictionary[audioName];
                }

                _globalBGMAudioSource2.Play();

                _globalBGMAudioSource1.DOFade(0, duration).OnComplete(() => _globalBGMAudioSource1.Stop());
                _globalBGMAudioSource2.DOFade(music.Volume, duration);
            }
            else
            {
                if (_currentBGM != null)
                {
                    if (!_musicTimeDictionary.ContainsKey(_currentBGM.Name))
                    {
                        _musicTimeDictionary.Add(_currentBGM.Name, _globalBGMAudioSource2.time);
                    }
                    else
                    {
                        _musicTimeDictionary[_currentBGM.Name] = _globalBGMAudioSource2.time;
                    }
                }

                _globalBGMAudioSource1.clip = music.GetClip();
                _globalBGMAudioSource1.pitch = music.Pitch + Random.Range(-music.PitchVariation * 0.5f, music.PitchVariation * 0.5f);
                _globalBGMAudioSource1.volume = 0f;

                if (continueIfPlayedBefore && _musicTimeDictionary.ContainsKey(audioName))
                {
                    _globalBGMAudioSource1.time = _musicTimeDictionary[audioName];
                }

                _globalBGMAudioSource1.Play();

                _globalBGMAudioSource2.DOFade(0, duration).OnComplete(() => _globalBGMAudioSource2.Stop());
                _globalBGMAudioSource1.DOFade(music.Volume, duration);
            }

            _currentBGM = music;
        }

        public async void StopBGM(float fadeOutTime = 0f)
        {
            if (!_globalBGMAudioSource1.isPlaying && !_globalBGMAudioSource2.isPlaying)
                return;

            if (_globalBGMAudioSource1.isPlaying)
            {
                if (fadeOutTime > 0f)
                {
                    await DOVirtual.Float(_globalBGMAudioSource1.volume, 0, fadeOutTime, (volume) => _globalBGMAudioSource1.volume = volume).AsyncWaitForCompletion();
                }

                _globalBGMAudioSource1.Stop();
                _currentBGM = null;
            }
            else
            {
                if (fadeOutTime > 0f)
                {
                    await DOVirtual.Float(_globalBGMAudioSource2.volume, 0, fadeOutTime, (volume) => _globalBGMAudioSource2.volume = volume).AsyncWaitForCompletion();
                }

                _globalBGMAudioSource2.Stop();
                _currentBGM = null;
            }

        }

        public void PlayGlobalSFX(string audioName)
        {
            if (_soundEffectsDictionary.ContainsKey(audioName))
            {
                Audio audio = _soundEffectsDictionary[audioName];

                _globalSFXAudioSource.volume = audio.Volume;
                _globalSFXAudioSource.pitch = audio.Pitch + Random.Range(-audio.PitchVariation * 0.5f, audio.PitchVariation * 0.5f);

                _globalSFXAudioSource.PlayOneShot(audio.GetClip());
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