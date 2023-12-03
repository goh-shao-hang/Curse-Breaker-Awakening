using CBA;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.Entities.Player
{
    public class PlayerHUDManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _playerHealthModule;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Image _healthBarRoot;
        [SerializeField] private Image _healthBarFill;
        [SerializeField] private Image _healthBarWhite;
        [SerializeField] private Image _staminaBarFill;
        [SerializeField] private Image _staminaBarWhite;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _shakeStrength = 5f;
        [SerializeField] private int _shakeVibrato = 5;
        [SerializeField] private float _tweenDuration = 0.5f;
        [SerializeField] private float _tweenDelay = 1f;

        private const float _staminaTweenDifferenceThreshold = 0.1f; //Minimum stamina percentage change to use tweening

        private void OnEnable()
        {
            _playerHealthModule.OnHealthChanged.AddListener(UpdateHealthUI);
            _playerController.OnStaminaChanged += UpdateStaminaUI;
        }

        private void OnDisable()
        {
            _playerController.OnStaminaChanged -= UpdateStaminaUI;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                _healthBarRoot.rectTransform.DOKill();
                _healthBarRoot.rectTransform.DOShakeAnchorPos(0.5f, _shakeStrength, _shakeVibrato);
            }
        }

        private void UpdateHealthUI()
        {
            float healthPercentage = Mathf.Clamp01(_playerHealthModule.CurrentHealth / _playerHealthModule.MaxHealth);

            _healthBarRoot.rectTransform.DOKill();
            _healthBarRoot.rectTransform.DOShakeAnchorPos(0.5f, _shakeStrength, _shakeVibrato);

            _healthBarFill.fillAmount = healthPercentage;

            _healthBarWhite.DOKill();
            _healthBarWhite.DOFillAmount(healthPercentage, _tweenDuration).SetEase(Ease.OutExpo).SetDelay(_tweenDelay);
            //_healthBarFill.fillAmount = healthPercentage;
        }

        private void UpdateStaminaUI(float staminaPercentage)
        {
            staminaPercentage = Mathf.Clamp01(staminaPercentage);

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
    }
}