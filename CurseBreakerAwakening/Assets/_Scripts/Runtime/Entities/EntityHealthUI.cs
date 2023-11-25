using CBA.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.Entities
{
    public class EntityHealthUI : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Entity _enemy;
        [SerializeField] private Image _healthBarFill;
        [SerializeField] private Image _staminaBarFill;

        private void OnEnable()
        {
            //_enemy.OnStaminaChanged += UpdateStaminaUI;
        }

        private void OnDisable()
        {
            //_enemy.OnStaminaChanged -= UpdateStaminaUI;
        }

        private void UpdateHealthUI(float healthPercentage)
        {
            healthPercentage = Mathf.Clamp01(healthPercentage);
            _healthBarFill.fillAmount = healthPercentage;
        }

        private void UpdateStaminaUI(float staminaPercentage)
        {
            staminaPercentage = Mathf.Clamp01(staminaPercentage);
            _staminaBarFill.fillAmount = staminaPercentage;
        }
    }
}