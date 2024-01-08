using DG.Tweening;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CBA.Core
{
    [DefaultExecutionOrder(-100)]
    public class GameManager : Singleton<GameManager>
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Level[] _levels;
        [SerializeField] private SceneField _endingScene;

        [Space]
        [SerializeField] private PlayerManager _playerManagerPrefab;

        public event Action OnPlayerDeath;

        private PlayerManager _playerManager;
        public PlayerManager PlayerManager
        {
            get
            {
                if (_playerManager == null)
                {
                    _playerManager = Instantiate(_playerManagerPrefab, Vector3.zero, Quaternion.identity);
                    _playerManager.ActivateComponents(true);
                    DontDestroyOnLoad(_playerManager.gameObject);
                    Debug.LogWarning($"Player spawned due to being accessed without starting a run. Make sure this is for testing only.");
                }
                return _playerManager;
            }
        }

        private int _currentLevel = 1;

        public int CurrentCoins { get; private set; } = 0;

        protected override void Awake()
        {
            base.Awake();

            this.SetDontDestroyOnLoad(true);
        }

        public async void StartRun(float delay = 0f)
        {
            if (delay > 0f)
                await Task.Delay((int)(delay * 1000f));

            AudioManager.Instance.PlayGlobalSFX("GameStartNarration");

            _currentLevel = 1;

            _playerManager = Instantiate(_playerManagerPrefab, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(_playerManager.gameObject);
            _playerManager.ActivateComponents(false);

            DOVirtual.DelayedCall(GameData.GAME_START_NARRATION_DELAY, () => LoadNextLevel());
        }

        public void EndLevel()
        {
            AudioManager.Instance.StopBGM(2f);

            _currentLevel++;

            if (_currentLevel > _levels.Length) //Last level completed
            {
                LoadEnding();
            }
            else
            {
                LoadNextLevel();
            }
        }

        public void LoadNextLevel()
        {
            PlayerManager.ActivateComponents(false);
            SceneTransitionManager.Instance.LoadSceneWithTransition(_levels[_currentLevel - 1].FloorScene, true);
        }

        public void LoadEnding()
        {
            Destroy(_playerManager.gameObject);

            SceneTransitionManager.Instance.LoadSceneWithTransition(_endingScene, false);
        }

        public void EnterBossRoom()
        {
            AudioManager.Instance.StopBGM(2f);

            SceneTransitionManager.Instance.LoadSceneWithTransition(_levels[_currentLevel - 1].BossScene, true);
        }

        public void PlayerDeath()
        {
            _playerManager.ActivateComponents(false);
            AudioManager.Instance.StopBGM(2f);
            AudioManager.Instance.PlayGlobalSFX("Player_Death");

            OnPlayerDeath?.Invoke();
        }

        public void ObtainCoin(int amount)
        {
            CurrentCoins += amount;
            Debug.Log($"You have {CurrentCoins} coins.");
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
                this.LevelName = $"Floor {index + 1}";
            }
        }
    }
}