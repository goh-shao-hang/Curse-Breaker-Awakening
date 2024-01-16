using CBA.Core;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace CBA
{
    public class IntroCutsceneTransition : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _cutscenePlayable;

        [SerializeField] private float _minPlayTime = 5f;

        private float _currentPlaytime;
        private bool _sceneLoading = false;

        private void Start()
        {
            _cutscenePlayable.stopped += OnCutsceneEnded;

            _currentPlaytime = 0f;
        }

        private void Update()
        {
            if (_sceneLoading)
                return;

            if (_currentPlaytime < _minPlayTime)
            {
                _currentPlaytime += Time.deltaTime;
                return;
            }

            //TODO
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                _cutscenePlayable.Stop();
            }
        }

        private void OnCutsceneEnded(PlayableDirector director)
        {
            StartRun();
        }

        private void StartRun()
        {
            if (_sceneLoading)
                return;

            _sceneLoading = true;
            GameManager.Instance.StartRunWithTutorial();
        }
    }
}