using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private SO_WeaponData _weaponData;
        [SerializeField] private WeaponAnimationEventHander _weaponAnimationEventHander;
        [SerializeField] private Animator _weaponAnimator;
        [SerializeField] private BoxCollider _hitbox;
        [SerializeField] private LayerMask _enemyLayer;

        public WeaponAnimationEventHander WeaponAnimationEventHander => _weaponAnimationEventHander;

        private List<Collider> _hitTargetCache;

        private int _currentCombo = 0;
        private bool _hitboxEnabled;
        private bool _allowNextComboInput;
        

        private void Awake()
        {
            _hitTargetCache = new List<Collider>();
            _allowNextComboInput = true;
        }

        private void OnEnable()
        {
            _weaponAnimationEventHander.OnActivateHitboxEvent += ActivateHitbox;
            _weaponAnimationEventHander.OnDeactivateHitboxEvent += DeactivateHitbox;
            _weaponAnimationEventHander.OnAllowNextComboEvent += AllowNextComboInput;
        }

        private void OnDisable()
        {
            _weaponAnimationEventHander.OnActivateHitboxEvent -= ActivateHitbox;
            _weaponAnimationEventHander.OnDeactivateHitboxEvent -= DeactivateHitbox;
            _weaponAnimationEventHander.OnAllowNextComboEvent -= AllowNextComboInput;
        }

        private void Update()
        {
            if (!_hitboxEnabled)
                return;

            Collider[] colliders = Physics.OverlapBox(_hitbox.transform.position, _hitbox.size * 0.5f, _hitbox.transform.rotation, GameData.ENEMY_LAYER);
            foreach (var collider in colliders)
            {
                if (_hitTargetCache.Contains(collider))
                {
                    continue;
                }

                _hitTargetCache.Add(collider);

                collider.GetComponent<Entity>()?.TakeDamage(1);

                //TODO
                Debug.LogWarning($"hit {collider.name}");
            }
        }

        public void Attack()
        {
            if (!_allowNextComboInput)
                return;

            _weaponAnimator.SetInteger(GameData.COMBO_HASH, _currentCombo);
            _weaponAnimator.SetTrigger(GameData.ATTACK_HASH);

            _allowNextComboInput = false;
            _currentCombo = (_currentCombo + 1) % (_weaponData.MaxCombo - 1);
        }

        public void ActivateHitbox()
        {
            _hitboxEnabled = true;
        }

        public void DeactivateHitbox()
        {
            _hitboxEnabled = false;

            //Reset cache
            _hitTargetCache.Clear();
        }

        public void AllowNextComboInput()
        {
            _allowNextComboInput = true;
        }

        public void ResetCombo()
        {
            _currentCombo = 0;
        }
    }
}