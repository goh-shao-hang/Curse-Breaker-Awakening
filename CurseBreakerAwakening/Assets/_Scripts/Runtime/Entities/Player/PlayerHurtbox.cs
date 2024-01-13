using CBA.Core;
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

        private DamageData _parryDamageData;

        private void OnEnable()
        {
            _healthModule.OnHealthDepleted.AddListener(PlayerDeath);
        }

        private void OnDisable()
        {
            _healthModule.OnHealthDepleted.RemoveListener(PlayerDeath);
        }

        private void PlayerDeath()
        {
            GameManager.Instance?.PlayerDeath();
        }

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

        public void TakeDamage(DamageData damageData)
        {
            if (IsParrying && _playerController.CurrentStamina > 0)
            {
                _parryDamageData.Set(_playerCombatManager.CurrentWeapon.WeaponData.ParryGuardDamage, this.gameObject, false, true); 

                damageData.Attacker.GetComponent<IDamageable>()?.TakeDamage(_parryDamageData);
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
                _healthModule.TakeDamage(damageData.DamageAmount);
            }
        }

    }
}