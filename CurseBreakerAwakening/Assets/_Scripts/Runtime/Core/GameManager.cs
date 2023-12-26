using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CBA.Core
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private SceneField _testSceneToLoad;

        protected override void Awake()
        {
            base.Awake();

            this.SetDontDestroyOnLoad(true);
        }

        public void StartRun()
        {
            AudioManager.Instance.PlayGlobalSFX("GameStartNarration");
            DOVirtual.DelayedCall(GameData.LEVEL_TRANSITION_TIME, LoadLevel);
        }

        private void LoadLevel()
        {
            SceneTransitionManager.Instance.TransitionToScene(_testSceneToLoad, true);
        }
    }
}