using CBA;
using CBA.Core;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.Entities.Player
{
    public class PlayerHUDManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _playerHealthModule;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerCameraController _playerCameraController;

        [Header("Health")]
        [SerializeField] private Image _healthBarRoot;
        [SerializeField] private Image _healthBarFill;
        [SerializeField] private Image _healthBarWhite;

        [Header("Stamina")]
        [SerializeField] private Image _staminaBarFill;
        [SerializeField] private Image _staminaBarWhite;

        [Header("Entity Info")]
        [SerializeField] private CanvasGroup _entityInfoCanvasGroup;
        [SerializeField] private TMP_Text _entityName;
        [SerializeField] private Image _entityHealthValue;
        [SerializeField] private RectTransform _entityGuardRoot;
        [SerializeField] private Image _entityGuardValue;

        [Header("Currency")]
        [SerializeField] private RawImage _coins;
        [SerializeField] private TMP_Text _coinText;

        [Header("Crosshair")]
        [SerializeField] private CanvasGroup _crossHairCanvasGroup;
        private PlayerGrabManager _playerGrabManager;

        [Header(GameData.SETTINGS)]
        [ColorUsage(true, true)] [SerializeField] private Color _healColor;
        [SerializeField] private float _shakeStrength = 5f;
        [SerializeField] private int _shakeVibrato = 5;
        [SerializeField] private float _tweenDuration = 0.5f;
        [SerializeField] private float _tweenDelay = 1f;

        private const float _staminaTweenDifferenceThreshold = 0.1f; //Minimum stamina percentage change to use tweening

        private const float _crosshairTweenDuration = 0.2f;

        private void Awake()
        {
            _playerGrabManager = _playerController.GetComponent<PlayerGrabManager>();
        }

        private void OnEnable()
        {
            _playerHealthModule.OnHealthIncreased.AddListener(OnHealthIncrease);
            _playerHealthModule.OnHealthDecreased.AddListener(OnHealthDecrease);
            _playerController.OnStaminaChanged += UpdateStaminaUI;
            _playerGrabManager.OnGrab += ShowCrosshair;
            _playerGrabManager.OnThrow += HideCrosshair;

            GameManager.Instance.OnCoinChanged += UpdateCoins;
        }

        private void OnDisable()
        {
            _playerHealthModule.OnHealthIncreased.AddListener(OnHealthIncrease);
            _playerHealthModule.OnHealthDecreased.RemoveListener(OnHealthDecrease);
            _playerController.OnStaminaChanged -= UpdateStaminaUI;
            _playerGrabManager.OnGrab -= ShowCrosshair;
            _playerGrabManager.OnThrow -= HideCrosshair;
        }

        private void OnHealthIncrease()
        {
            float healthPercentage = Mathf.Clamp01(_playerHealthModule.CurrentHealth / _playerHealthModule.MaxHealth);

            _healthBarWhite.DOKill(true);
            _healthBarFill.DOKill(true);

            _healthBarWhite.color = _healColor;
            _healthBarWhite.fillAmount = healthPercentage;

            _healthBarFill.DOFillAmount(healthPercentage, _tweenDuration).SetEase(Ease.OutExpo).SetDelay(_tweenDelay);

            _healthBarRoot.DOKill();
            _healthBarRoot.color = Color.white;

            _healthBarRoot.DOColor(_healColor, 0.25f).SetLoops(2, LoopType.Yoyo).OnComplete(() => LowHealthWarning(healthPercentage));
        }

        private void OnHealthDecrease()
        {
            float healthPercentage = Mathf.Clamp01(_playerHealthModule.CurrentHealth / _playerHealthModule.MaxHealth);

            _healthBarRoot.rectTransform.DOKill(true);
            _healthBarRoot.DOKill();
            _healthBarRoot.color = Color.white;

            _healthBarWhite.DOKill(true);
            _healthBarFill.DOKill(true);

            _healthBarRoot.rectTransform.DOShakeAnchorPos(0.5f, _shakeStrength, _shakeVibrato);

            _healthBarFill.fillAmount = healthPercentage;

            _healthBarWhite.color = Color.white;
            _healthBarWhite.DOFillAmount(healthPercentage, _tweenDuration).SetEase(Ease.OutExpo).SetDelay(_tweenDelay);

            _healthBarRoot.DOColor(Color.red, 0.25f).SetLoops(2, LoopType.Yoyo).OnComplete(() => LowHealthWarning(healthPercentage));
        }

        private void LowHealthWarning(float healthPercentage)
        {
            if (healthPercentage <= 0.5f)
            {
                _healthBarRoot.DOColor(Color.red, 0.5f).SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void UpdateStaminaUI()
        {
            float staminaPercentage = Mathf.Clamp01(_playerController.CurrentStamina / _playerController.MaxStamina);

            if (staminaPercentage < _staminaBarFill.fillAmount)
            {
                _staminaBarWhite.DOKill();
                _staminaBarWhite.DOFillAmount(staminaPercentage, _tweenDuration).SetEase(Ease.OutExpo).SetDelay(_tweenDelay);
            }
            else
            {
                _staminaBarWhite.fillAmount = staminaPercentage;
            }
            
            _staminaBarFill.fillAmount = staminaPercentage;
        }

        #region Coin Variables
        private bool _coinShowing = false;
        private const float _coinHiddenYPos = -150;
        private const float _coinShownYPos = 75;
        private Tween _coinScaleTween;
        private Tween _hideCoinsTween;
        #endregion
        private void UpdateCoins()
        {
            if (_coinScaleTween != null)
                _coinScaleTween.Kill(true);

            _coinScaleTween = _coins.rectTransform.DOScale(1.25f, 0.1f).SetLoops(2, LoopType.Yoyo);

            if (!_coinShowing)
                _coins.rectTransform.DOAnchorPosY(_coinShownYPos, 0.5f);

            _coinShowing = true;
            _coinText.SetText(GameManager.Instance.CurrentCoins.ToString());

            if (_hideCoinsTween != null)
                _hideCoinsTween.Kill();

            _hideCoinsTween = _coins.rectTransform.DOAnchorPosY(_coinHiddenYPos, 0.5f).SetDelay(3.5f).OnPlay(() => _coinShowing = false);
        }

        private void ShowCrosshair()
        {
            _crossHairCanvasGroup.DOKill(true);
            _crossHairCanvasGroup.DOFade(1, _crosshairTweenDuration).SetEase(Ease.OutSine);
            _crossHairCanvasGroup.transform.DOScale(1, _crosshairTweenDuration).SetEase(Ease.OutSine);
        }

        private void HideCrosshair()
        {
            _crossHairCanvasGroup.DOFade(0, _crosshairTweenDuration).SetEase(Ease.OutSine);
            _crossHairCanvasGroup.transform.DOScale(2, _crosshairTweenDuration).SetEase(Ease.OutSine);
        }

        public void ShowEntityInfo(EntityInfo info)
        {
            _entityInfoCanvasGroup.alpha = 1;

            _entityName.text = info.EntityName;
            _entityHealthValue.fillAmount = info.HealthModule.CurrentHealth / info.HealthModule.MaxHealth;

            if (info.GuardModule != null)
            {
                _entityGuardRoot.gameObject.SetActive(true);

                _entityGuardValue.fillAmount = info.GuardModule.CurrentGuardMeter / info.GuardModule.MaxGuardMeter;
            }
            else
            {
                _entityGuardRoot.gameObject.SetActive(false);
            }
        }

        public void HideEntityInfo()
        {
            _entityInfoCanvasGroup.alpha = 0;
        }

    }
}