using CBA.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace CBA
{
    public class IntroCutsceneTransition : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _cutscenePlayable;
        [SerializeField] private CanvasGroup _skipCanvasGroup;
        [SerializeField] private Image _skipMask;

        [SerializeField] private float _holdTimeForSkip = 3f;
        [SerializeField] private float _minPlayTime = 5f;

        private float _currentPlaytime;
        private bool _sceneLoading = false;

        private InputAction _cancelAction;

        private bool _isSkipping = false;
        private bool _skipFinished = false;

        private float _skipProgress = 0f;

        private void Start()
        {
            _cutscenePlayable.stopped += OnCutsceneEnded;

            _currentPlaytime = 0f;

            _skipCanvasGroup.alpha = 0f;
        }

        private void OnEnable()
        {
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
            if (_sceneLoading)
                return;

            if (_cutscenePlayable.time < _minPlayTime)
            {
                _currentPlaytime += Time.deltaTime;
                return;
            }

            if (_isSkipping && ! _skipFinished)
            {
                _skipProgress += Time.deltaTime;
                _skipMask.fillAmount = _skipProgress / _holdTimeForSkip;

                if (_skipProgress >= _holdTimeForSkip)
                {
                    //Skip
                    _cutscenePlayable.Stop();
                    _skipFinished = true;
                }
            }

            /*if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                _cutscenePlayable.Stop();
            }*/
        }

        private void CancelActionHeld(InputAction.CallbackContext ctx)
        {
            if (_skipFinished) return;

            _isSkipping = true;
            _skipProgress = 0f;

            _skipMask.DOKill();
            _skipCanvasGroup.DOKill();

            _skipCanvasGroup.DOFade(1f, 0.5f);
        }

        private void CancelActionReleased(InputAction.CallbackContext ctx)
        {
            if (_skipFinished) return;

            _isSkipping = false;
            _skipProgress = 0f;

            _skipMask.DOKill();
            _skipCanvasGroup.DOKill();

            _skipMask.DOFillAmount(0f, 0.5f).SetEase(Ease.OutSine);
            _skipCanvasGroup.DOFade(0f, 1f);
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