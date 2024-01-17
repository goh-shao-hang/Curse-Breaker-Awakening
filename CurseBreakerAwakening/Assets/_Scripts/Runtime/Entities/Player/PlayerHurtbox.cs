using CBA.Core;
using CBA.Entities;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace CBA.Entities.Player
{
    public class PlayerHurtbox : MonoBehaviour, IDamageable
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _healthModule;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerCombatManager _playerCombatManager;
        [SerializeField] private Volume _playerHurtVolume;

        public bool IsBlocking { get; private set; }
        public bool IsParrying { get; private set; }

        public UnityEvent OnParrySuccess;
        public UnityEvent OnBlockSuccess;

        private DamageData _parryDamageData;

        private Tween _hurtVolumeTween;

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

                if (damageData.Attacker != null) //Dont deal parry damage to things like projectile
                {
                    damageData.Attacker.GetComponent<IDamageable>()?.TakeDamage(_parryDamageData);
                }

                GameEventsManager.Instance?.CameraShake(Vector3.one, 0.3f);

                OnParrySuccess?.Invoke();
            }
            else if (IsBlocking && _playerController.CurrentStamina > 0)
            {
                _playerController.SetStamina(_playerController.CurrentStamina - _playerCombatManager.BlockStaminaConsumption);
                _playerController.StartStaminaRegenTimer();

                OnBlockSuccess?.Invoke();

                GameEventsManager.Instance?.CameraShake(Vector3.one, 0.3f);

                if (_playerController.CurrentStamina <= 0) //Stamina depleted after taking this hit
                {
                    _playerCombatManager.InterruptBlocking();
                    //TODO some warning effect
                }
            }
            else
            {
                GameEventsManager.Instance?.CameraShake(Vector3.one, 1f);

                _healthModule.TakeDamage(damageData.DamageAmount);

                if (_playerHurtVolume != null)
                {
                    if (_hurtVolumeTween != null)
                        _hurtVolumeTween.Kill();

                    _playerHurtVolume.weight = 0f;

                    _hurtVolumeTween = DOVirtual.Float(0, 1, 0.5f, (weight) => _playerHurtVolume.weight = weight).SetLoops(2, LoopType.Yoyo);
                }
            }
        }

    }
}