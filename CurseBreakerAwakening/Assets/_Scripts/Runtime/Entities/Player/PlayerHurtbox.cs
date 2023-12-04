using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerHurtbox : MonoBehaviour, IDamageable
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _healthModule;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerCombatManager _playerCombatManager;

        public void TakeDamage(float amount)
        {
            if (_playerCombatManager.IsBlocking && _playerController.CurrentStamina > 0)
            {
                _playerController.SetStamina(_playerController.CurrentStamina - _playerCombatManager.BlockStaminaConsumption);
                _playerController.StartStaminaRegenTimer();

                if (_playerController.CurrentStamina <= 0) //Stamina depleted after taking this hit
                {
                    _playerCombatManager.StopBlocking();
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