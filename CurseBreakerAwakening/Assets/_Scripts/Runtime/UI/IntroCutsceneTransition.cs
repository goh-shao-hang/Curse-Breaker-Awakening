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

        private void Start()
        {
            _cutscenePlayable.stopped += OnCutsceneEnded;
        }

        private void Update()
        {
            //TODO
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                _cutscenePlayable.Stop();
                StartRun();
            }
        }

        private void OnCutsceneEnded(PlayableDirector director)
        {
            StartRun();
        }

        private void StartRun()
        {
            GameManager.Instance.StartRun(5f);
        }
    }
}