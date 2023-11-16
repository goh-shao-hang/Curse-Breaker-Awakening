using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        //TODO
        [SerializeField] private GameObject _hitVFX;

        public WeaponAnimationEventHander WeaponAnimationEventHander => _weaponAnimationEventHander;

        private List<Collider> _hitTargetCache;

        private int _currentCombo = 0;
        private bool _hitboxEnabled;

        public bool NextComboInputAllowed { get; private set; }

        //Events
        public event Action OnWeaponHit;

        private void Awake()
        {
            _hitTargetCache = new List<Collider>();
            NextComboInputAllowed = true;
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_hitbox.transform.position, _hitbox.transform.position + _hitbox.transform.forward * _hitbox.size.z);
        }

        private void Update()
        {
            if (!_hitboxEnabled)
                return;

            /*if (Physics.Raycast(_hitbox.transform.position, _hitbox.transform.up, out RaycastHit hit, _hitbox.size.z, GameData.ENEMY_LAYER))
            {
                if (_hitTargetCache.Contains(hit.collider))
                    return;

                _hitTargetCache.Add(hit.collider);

                hit.collider.GetComponent<Entity>()?.TakeDamage(1);

                OnWeaponHit?.Invoke();

                //VFX
                if (_hitVFX != null)
                {
                    Instantiate(_hitVFX, hit.point, Quaternion.identity);
                }

                //TODO
                Debug.LogWarning($"hit {hit.collider.name}");
            }*/

            Collider[] colliders = Physics.OverlapBox(_hitbox.transform.position, _hitbox.size * 0.5f, _hitbox.transform.rotation, GameData.ENEMY_LAYER);
            foreach (var collider in colliders)
            {
                if (_hitTargetCache.Contains(collider))
                {
                    continue;
                }

                _hitTargetCache.Add(collider);

                collider.GetComponent<Entity>()?.TakeDamage(1);

                OnWeaponHit?.Invoke();

                //VFX
                if (_hitVFX != null)
                {
                    Instantiate(_hitVFX, _hitbox.transform.position, Quaternion.identity);
                }

                //TODO
                Debug.LogWarning($"hit {collider.name}");
            }
        }

        public void Attack()
        {
            if (!NextComboInputAllowed)
                return;

            _weaponAnimator.SetInteger(GameData.COMBO_HASH, _currentCombo);
            _weaponAnimator.SetTrigger(GameData.ATTACK_HASH);

            NextComboInputAllowed = false;
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
            NextComboInputAllowed = true;
        }

        public void ResetCombo()
        {
            _currentCombo = 0;
        }
    }
}