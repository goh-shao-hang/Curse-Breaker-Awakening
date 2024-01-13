using CBA.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.Entities
{
    public class EntityUI : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Entity _entity; 
        [SerializeField] private Image _healthBarFill;
        [SerializeField] private Image _guardBarFill;

        private HealthModule _healthModule;
        private HealthModule healthModule => _healthModule ??= _entity.GetModule<HealthModule>();

        private GuardModule _guardModule;
        private GuardModule guardModule => _guardModule ??= _entity.GetModule<GuardModule>();

        private void OnEnable()
        {
            if (healthModule)
            {
                _healthModule.OnHealthIncreased.AddListener(UpdateHealthUI);
                _healthModule.OnHealthDecreased.AddListener(UpdateHealthUI);
            }

            if (guardModule)
            {
                _guardModule.OnGuardMeterIncreased.AddListener(UpdateGuardUI);
                _guardModule.OnGuardMeterDecreased.AddListener(UpdateGuardUI);
            }
        }

        private void OnDisable()
        {
            if (healthModule)
            {
                _healthModule.OnHealthIncreased.RemoveListener(UpdateHealthUI);
                _healthModule.OnHealthDecreased.RemoveListener(UpdateHealthUI);
            }

            if (guardModule)
            {
                _guardModule.OnGuardMeterIncreased.RemoveListener(UpdateGuardUI);
                _guardModule.OnGuardMeterDecreased.RemoveListener(UpdateGuardUI);
            }
        }

        private void UpdateHealthUI()
        {
            float healthPercentage = Mathf.Clamp01(healthModule.CurrentHealth / healthModule.MaxHealth);
            _healthBarFill.fillAmount = healthPercentage;
        }

        private void UpdateGuardUI()
        {
            float guardPercentage = Mathf.Clamp01(guardModule.CurrentGuardMeter / guardModule.MaxGuardMeter);
            _guardBarFill.fillAmount = guardPercentage;
        }
    }
}