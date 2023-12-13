using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Entities.Player
{
    public class PlayerHurtbox : MonoBehaviour, IDamageable
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _healthModule;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerCombatManager _playerCombatManager;

        public bool IsBlocking { get; private set; }
        public bool IsParrying { get; private set; }

        public UnityEvent OnParrySuccess;
        public UnityEvent OnBlockSuccess;

        public void SetIsBlocking(bool isBlocking)
        {
            IsBlocking = isBlocking;
            CancelInvoke();
        }

        public void SetIsParrying(bool isParrying)
        {
            IsParrying = isParrying;
            CancelInvoke();
        }

        public void TakeDamage(float amount)
        {
            if (IsParrying && _playerController.CurrentStamina > 0)
            {
                OnParrySuccess?.Invoke();
            }
            else if (IsBlocking && _playerController.CurrentStamina > 0)
            {
                _playerController.SetStamina(_playerController.CurrentStamina - _playerCombatManager.BlockStaminaConsumption);
                _playerController.StartStaminaRegenTimer();

                OnBlockSuccess?.Invoke();

                if (_playerController.CurrentStamina <= 0) //Stamina depleted after taking this hit
                {
                    _playerCombatManager.InterruptBlocking();
                    //TODO some warning effect
                }
            }
            else
            {
                _healthModule.TakeDamage(amount);
            }
        }

    }
}