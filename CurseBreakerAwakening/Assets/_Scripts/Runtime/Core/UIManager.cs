using CBA.Core;
using CBA.Entities;
using CBA.Entities.Player;
using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[DefaultExecutionOrder(0)]
public class UIManager : Singleton<UIManager>
{
    [Header("Pause")]
    [SerializeField] private CanvasGroup _pauseCanvas;
    [SerializeField] private Image _playerHealthFill;
    [SerializeField] private Image _playerStaminaFill;
    [SerializeField] private TMP_Text _playerHealthText;
    [SerializeField] private TMP_Text _playerStaminaText;

    [Header("Dialog")]
    [SerializeField] private CanvasGroup _dialogCanvasGroup;
    [SerializeField] private TMP_Text _gabText;
    [SerializeField] private TMP_Text _dialogText;

    [Header("Death")]
    [SerializeField] private CanvasGroup _deathCanvas;
    [SerializeField] private CanvasGroup _deathScreenButtonsCanvas;
    [SerializeField] private Button[] _deathScreenButtons;
    [SerializeField] private TMP_Text _deathText;
    [SerializeField] private TMP_Text _deathText2;

    //TODO
    [SerializeField] private SceneField _mainMenuScene;

    private bool _canPause = true;
    private bool _paused = false;

    private PlayerController _playerController;
    private HealthModule _playerHealthModule;

    private SO_Dialog _currentDialog;
    private Coroutine _dialogCO;

    protected override void Awake()
    {
        base.Awake();

        //this.SetDontDestroyOnLoad(true);
    }

    private void OnEnable()
    {
        _pauseCanvas.gameObject.SetActive(false);
        _deathCanvas.gameObject.SetActive(false);

        GameManager.Instance.OnPlayerDeath += ShowDeathScreen;

        //TODO
        //GameManager.Instance.OnGameEnded += () => Destroy(this.gameObject);

        _playerController ??= GameManager.Instance.PlayerManager.PlayerController;
        _playerHealthModule ??= _playerController.GetComponentInChildren<HealthModule>();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDeath -= ShowDeathScreen;

            //TODO
            //GameManager.Instance.OnGameEnded -= () => Destroy(this.gameObject);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        if (!_canPause)
            return;

        _paused = !_paused;

        Helper.LockAndHideCursor(!_paused);
        Time.timeScale = _paused ? 0 : 1;

        _pauseCanvas.gameObject.SetActive(_paused);
        _pauseCanvas.alpha = _paused ? 1 : 0;
        _pauseCanvas.interactable = _paused;
        _pauseCanvas.blocksRaycasts = _paused;

        if (_paused)
        {
            UpdateHealth();
            UpdateStamina();
        }
    }

    private void UpdateHealth()
    {
        float healthPercentage = Mathf.Clamp01(_playerHealthModule.CurrentHealth / _playerHealthModule.MaxHealth);
        _playerHealthFill.fillAmount = healthPercentage;
        _playerHealthText.SetText($"{_playerHealthModule.CurrentHealth} / {_playerHealthModule.MaxHealth}");
    }

    private void UpdateStamina()
    {
        float staminaPercentage = Mathf.Clamp01(_playerController.CurrentStamina / _playerController.MaxStamina);
        _playerStaminaFill.fillAmount = staminaPercentage;
        _playerStaminaText.SetText($"{(int)_playerController.CurrentStamina} / {(int)_playerController.MaxStamina}");
    }

    public void StartDialog(SO_Dialog dialog)
    {
        if (_dialogCO != null)
            return;

        _dialogCanvasGroup.DOFade(1, 0.5f);

        _currentDialog = dialog;

        _dialogCO = StartCoroutine(DialogCO());
    }

    private IEnumerator DialogCO()
    {
        for (int i = 0; i < _currentDialog.Paragraphs.Length; i++)
        {
            _dialogText.text = _currentDialog.Paragraphs[i];
            yield return WaitHandler.GetWaitForSeconds(3f);
        }

        EndDialog();
    }


    public void EndDialog()
    {
        _dialogCanvasGroup.DOFade(0, 0.5f);
        _dialogCO = null;
    }

    public void ShowDeathScreen()
    {
        _deathCanvas.gameObject.SetActive(true);

        _canPause = false;

        Helper.LockAndHideCursor(false);

        var deathSequence = DOTween.Sequence();
        GameObject spacingTweenRef = new GameObject();

        _deathText.alpha = 0;
        _deathText2.alpha = 0;

        deathSequence.Append(_deathCanvas.DOFade(1, 1f));
        deathSequence.Join(_deathText.DOFade(1, 2f));
        deathSequence.Join(_deathText2.DOFade(0.08f, 2f));
        deathSequence.Join(spacingTweenRef.transform.DOLocalMoveX(23, 3f).OnUpdate(() => UpdateDeathTextSpacing(spacingTweenRef.transform.position.x)));
        deathSequence.AppendInterval(1f);
        deathSequence.Append(_deathText.rectTransform.DOAnchorPosY(250f, 1f).SetEase(Ease.OutSine).SetUpdate(true));
        deathSequence.Append(_deathScreenButtonsCanvas.DOFade(1, 1f));

        deathSequence.OnComplete(ActivateDeathScreenButtons);
    }

    private void ActivateDeathScreenButtons()
    {
        _deathCanvas.interactable = true;
        _deathScreenButtonsCanvas.interactable = true;
        _deathScreenButtonsCanvas.blocksRaycasts = true;

        EventSystem.current.GetComponent<InputSystemUIInputModule>().deselectOnBackgroundClick = false;
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

        GameManager.Instance.StartRun(5f);

        Destroy(this.gameObject);
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
