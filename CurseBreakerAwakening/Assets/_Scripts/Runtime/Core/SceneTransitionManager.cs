using CBA;
using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    [SerializeField] private CanvasGroup _transitionCanvasGroup;
    [SerializeField] private CanvasGroup _loadingBarCanvasGroup;
    [SerializeField] private Image _loadingBarFill;

    private Coroutine _loadSceneCO = null;

    private const float _loadingBarFadeDuration = 1f;

    protected override void Awake()
    {
        base.Awake();

        this.SetDontDestroyOnLoad(true);
    }


    public void LoadSceneWithTransition(SceneField scene, bool useLoadingBar = false)
    {
        if (_loadSceneCO != null)
        {
            Debug.LogError("A scene is already being loaded!");
            return;
        }

        _transitionCanvasGroup.blocksRaycasts = true;
        _transitionCanvasGroup.DOFade(1, GameData.SCENE_TRANSITION_DURATION).OnComplete(() => _loadSceneCO = StartCoroutine(LoadSceneCO(scene, useLoadingBar)));
    }

    private IEnumerator LoadSceneCO(SceneField scene, bool useLoadingBar)
    {
        if (useLoadingBar)
        {
            _loadingBarFill.fillAmount = 0f;
            _loadingBarCanvasGroup.DOFade(1, _loadingBarFadeDuration);
            yield return WaitHandler.GetWaitForSeconds(_loadingBarFadeDuration);
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scene);
        float progress = 0f;

        while (!loadOperation.isDone)
        {
            if (useLoadingBar)
            {
                progress = loadOperation.progress;
                _loadingBarFill.fillAmount = progress / 0.9f;
                Debug.Log(_loadingBarFill.fillAmount);
            }

            yield return null;
        }

        if (useLoadingBar)
        {
            _loadingBarCanvasGroup.DOFade(0, _loadingBarFadeDuration).OnComplete(() => _transitionCanvasGroup.DOFade(0, GameData.SCENE_TRANSITION_DURATION));
        }
        else
        {
            _transitionCanvasGroup.DOFade(0, GameData.SCENE_TRANSITION_DURATION);
        }

        yield return WaitHandler.GetWaitForSeconds(GameData.SCENE_TRANSITION_DURATION);
        _transitionCanvasGroup.blocksRaycasts = false;

        _loadSceneCO = null;
    }
}
