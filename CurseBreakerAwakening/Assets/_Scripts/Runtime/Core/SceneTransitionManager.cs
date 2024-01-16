using CBA;
using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    [SerializeField] private CanvasGroup _transitionCanvasGroup;
    [SerializeField] private CanvasGroup _loadingBarCanvasGroup;
    [SerializeField] private Image _loadingBarFill;
    [SerializeField] private TMP_Text _loadingScreenTipTitle;
    [SerializeField] private TMP_Text _loadingScreenTip;
    [SerializeField] private SO_LoadingScreenTip[] _loadingScreenTips;

    //Low priority cam that ensures audio can be heard. No sure if this is the best way tho.
    [SerializeField] private Camera _camera;

    private Coroutine _loadSceneCO = null;

    private const float _loadingBarFadeDuration = 1f;

    protected override void Awake()
    {
        base.Awake();

        this.SetDontDestroyOnLoad(true);
    }


    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnAnySceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnAnySceneUnloaded;
    }

    private void OnAnySceneUnloaded(Scene scene)
    {
        //TODO not sure if this is safe
        DOTween.KillAll();
    }

    public void LoadSceneWithTransition(SceneField scene, bool useLoadingBar = false)
    {
        if (_loadSceneCO != null)
        {
            Debug.LogError("A scene is already being loaded!");
            return;
        }

        _camera.gameObject.SetActive(true);

        _transitionCanvasGroup.blocksRaycasts = true;
        _transitionCanvasGroup.DOFade(1, GameData.SCENE_TRANSITION_DURATION).OnComplete(() => _loadSceneCO = StartCoroutine(LoadSceneCO(scene, useLoadingBar)));
    }

    private IEnumerator LoadSceneCO(SceneField scene, bool useLoadingBar)
    {
        if (useLoadingBar)
        {
            int random = Random.Range(0, _loadingScreenTips.Length);
            _loadingScreenTipTitle.text = _loadingScreenTips[random].TipTitle;
            _loadingScreenTip.text = _loadingScreenTips[random].TipText;

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
            }

            yield return null;
        }

        _camera.gameObject.SetActive(false);

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
