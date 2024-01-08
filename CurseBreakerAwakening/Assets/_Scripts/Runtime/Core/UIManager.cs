using CBA.Core;
using DG.Tweening;
using GameCells.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(0)]
public class UIManager : Singleton<UIManager>
{
    [Header("Pause")]
    [SerializeField] private CanvasGroup _pauseCanvas;

    [Header("Death")]
    [SerializeField] private CanvasGroup _deathCanvas;
    [SerializeField] private CanvasGroup _deathScreenButtonsCanvas;
    [SerializeField] private Button[] _deathScreenButtons;
    [SerializeField] private TMP_Text _deathText;
    [SerializeField] private TMP_Text _deathText2;

    //TODO
    [SerializeField] private SceneField _mainMenuScene;
    [SerializeField] private SceneField _emptyScene;

    private bool _canPause = true;
    private bool _paused = false;

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerDeath += ShowDeathScreen;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerDeath -= ShowDeathScreen;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            ShowDeathScreen();
        }
    }

    public void OpenPauseMenu()
    {
        if (!_canPause)
            return;

        _paused = !_paused;
        Helper.LockAndHideCursor(!_paused);
        Time.timeScale = _paused ? 0 : 1;

        _pauseCanvas.alpha = _paused ? 1 : 0;
        _pauseCanvas.interactable = _paused;
        _pauseCanvas.blocksRaycasts = _paused;
    }

    public void ShowDeathScreen()
    {
        _canPause = false;

        Helper.LockAndHideCursor(false);

        var deathSequence = DOTween.Sequence();
        GameObject spacingTweenRef = new GameObject();

        deathSequence.Append(_deathCanvas.DOFade(1, 1f));
        deathSequence.Join(_deathText.DOFade(1, 1f));
        deathSequence.Join(spacingTweenRef.transform.DOLocalMoveX(23, 5f).OnUpdate(() => UpdateDeathTextSpacing(spacingTweenRef.transform.position.x)));
        deathSequence.AppendInterval(1f);
        deathSequence.Append(_deathText.rectTransform.DOAnchorPosY(250f, 1.5f).SetEase(Ease.OutSine).SetUpdate(true));
        deathSequence.Append(_deathScreenButtonsCanvas.DOFade(1, 1f));

        deathSequence.OnComplete(ActivateDeathScreenButtons);
    }

    private void ActivateDeathScreenButtons()
    {
        _deathCanvas.interactable = true;
        _deathScreenButtonsCanvas.interactable = true;
        _deathScreenButtonsCanvas.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(_deathScreenButtons[0].gameObject);
    }

    private void UpdateDeathTextSpacing(float spacing)
    {
        _deathText.characterSpacing = spacing;
        _deathText2.characterSpacing = spacing;
    }

    public void QuickRestart()
    {
        _deathScreenButtonsCanvas.interactable = false;

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);
        SceneTransitionManager.Instance.LoadSceneWithTransition(_emptyScene, false);

        //TODO
        GameManager.Instance.StartRun(5f);
    }

    public void ReturnToMainMenu()
    {
        _deathScreenButtonsCanvas.interactable = false;

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);

        SceneTransitionManager.Instance.LoadSceneWithTransition(_mainMenuScene, false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
