using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.Entities.Player
{
    public class PlayerHUDManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PlayerStamina _playerStamina;
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private Image _staminaFillImage;

        private void OnEnable()
        {
            _playerStamina.OnCurrentStaminaChanged += UpdateStaminaUI;
        }

        private void OnDisable()
        {

            _playerStamina.OnCurrentStaminaChanged -= UpdateStaminaUI;
        }

        public void UpdateHealthUI()
        {

        }

        public void UpdateStaminaUI()
        {
            _staminaFillImage.fillAmount = _playerStamina.CurrentStamina / _playerStamina.MaxStamina;
        }
    }
}