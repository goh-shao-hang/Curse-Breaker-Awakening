using DG.Tweening;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CBA.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Level[] _levels;

        private int _currentLevel = 1;

        protected override void Awake()
        {
            base.Awake();

            this.SetDontDestroyOnLoad(true);
        }

        public void StartRun()
        {
            AudioManager.Instance.PlayGlobalSFX("GameStartNarration");

            _currentLevel = 1;
            DOVirtual.DelayedCall(GameData.GAME_START_NARRATION_DELAY, () => LoadNextLevel());
        }

        public void LoadNextLevel()
        {
            SceneTransitionManager.Instance.LoadSceneWithTransition(_levels[_currentLevel - 1].FloorScene, true);
        }

        public void EnterBossRoom()
        {
            SceneTransitionManager.Instance.LoadSceneWithTransition(_levels[_currentLevel - 1].BossScene, true);
        }

        private void OnValidate()
        {
            for (int i = 0; i < _levels.Length; i++)
            {
                _levels[i].UpdateLevelNames(i);
            }
        }

        [Serializable]
        public class Level
        {
            [HideInInspector] public string LevelName;

            [field: SerializeField] public SceneField FloorScene;
            [field: SerializeField] public SceneField BossScene;

            public void UpdateLevelNames(int index)
            {
                this.LevelName = $"Floor {index}";
            }
        }
    }
}