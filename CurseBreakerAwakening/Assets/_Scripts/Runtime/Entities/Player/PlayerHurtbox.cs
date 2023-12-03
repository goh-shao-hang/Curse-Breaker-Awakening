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
            _healthModule.TakeDamage(amount);

            //TODO set up guarding
            /*if ( _playerController.CurrentStamina > 0)
            {
                _playerController.SetStamina(_playerController.CurrentStamina - amount);
                _playerController.StartStaminaRegenTimer();

                //TODO make some warning if stamina is 0
            }
            else
            {
                _healthModule.TakeDamage(amount);
            }*/
        }
    }
}