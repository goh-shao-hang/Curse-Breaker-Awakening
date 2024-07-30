using CBA;
using CBA.Core;
using DG.Tweening;
using GameCells.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.Video;

public class Ending : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private SceneField _mainMenuScene;
    [SerializeField] private ButtonScrollList _buttons;
    [SerializeField] private CanvasGroup _videoPlayerCanvas;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private CanvasGroup _skipCanvasGroup;
    [SerializeField] private Image _skipMask;
    [SerializeField] private Image _transitionFadeImage;

    [Header(GameData.SETTINGS)]
    [SerializeField] private float _trailerStartDelay = 3f;
    [SerializeField] private float _minTrailerPlayerTime = 1f;

    private InputAction _cancelAction;

    private bool _isSkipping = false;
    private bool _trailerEnded = false;

    private float _skipProgress = 0f;

    private void Start()
    {
        _skipCanvasGroup.alpha = 0f;
        _buttons.DisableButtons();

        DOVirtual.DelayedCall(_trailerStartDelay, StartPlayingTrailer);
    }

    private void OnEnable()
    {
        _videoPlayer.loopPointReached += OnTrailerEnded;

        _cancelAction ??= EventSystem.current.GetComponent<InputSystemUIInputModule>().actionsAsset.FindAction("Cancel");
        _cancelAction.performed += CancelActionHeld;
        _cancelAction.canceled += CancelActionReleased;
    }

    private void OnDisable()
    {
        _cancelAction.performed -= CancelActionHeld;
        _cancelAction.canceled -= CancelActionReleased;
    }

    private void Update()
    {
        if (_trailerEnded)
            return;

        if (_videoPlayer.time < _minTrailerPlayerTime)
        {
            return;
        }

        if (_isSkipping)
        {
            _skipProgress += Time.deltaTime;
            _skipMask.fillAmount = _skipProgress / GameData.CUTSCENE_SKIP_TIME;

            if (_skipProgress >= GameData.CUTSCENE_SKIP_TIME)
            {
                //Skip
                _videoPlayer.Stop();
                OnTrailerEnded(_videoPlayer);
            }
        }
    }

    private void StartPlayingTrailer()
    {
        _videoPlayerCanvas.gameObject.SetActive(true);
        _skipCanvasGroup.gameObject.SetActive(true);

        _videoPlayer.Play();
    }

    //TODO duplicate code from intro cutscene skip functionality
    private void CancelActionHeld(InputAction.CallbackContext ctx)
    {
        if (_trailerEnded) return;

        _isSkipping = true;
        _skipProgress = 0f;

        _skipMask.DOKill();
        _skipCanvasGroup.DOKill();

        _skipCanvasGroup.DOFade(1f, 0.5f);
    }

    private void CancelActionReleased(InputAction.CallbackContext ctx)
    {
        if (_trailerEnded) return;

        _isSkipping = false;
        _skipProgress = 0f;

        _skipMask.DOKill();
        _skipCanvasGroup.DOKill();

        _skipMask.DOFillAmount(0f, 0.5f).SetEase(Ease.OutSine);
        _skipCanvasGroup.DOFade(0f, 1f);
    }

    private void OnTrailerEnded(VideoPlayer source)
    {
        _videoPlayer.loopPointReached -= OnTrailerEnded;
        _trailerEnded = true;

        _videoPlayerCanvas.DOFade(0, 1f);
        _skipCanvasGroup.DOFade(0, 1f);

        _transitionFadeImage.color = Color.black;
        _transitionFadeImage.DOFade(0, 1f).SetDelay(2f).OnComplete(ShowEndingScreen);
    }

    private void ShowEndingScreen()
    {
        AudioManager.Instance.PlayBGM("EndingTheme");
        _buttons.ShowButtons();

        Helper.LockAndHideCursor(false);
    }

    public void ReturnToMainMenu()
    {
        _buttons.SetInteractable(false);

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);

        SceneTransitionManager.Instance.LoadSceneWithTransition(_mainMenuScene, false);
    }

    public void QuickRestart()
    {
        _buttons.SetInteractable(false);

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);

        GameManager.Instance.StartRun(5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
