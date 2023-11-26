using Cinemachine;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.VFX;

namespace CBA.Entities.Player.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private SO_WeaponData _weaponData;
        [SerializeField] private WeaponAnimationEventHander _weaponAnimationEventHander;
        [SerializeField] private Animator _weaponAnimator;
        [SerializeField] private BoxCollider _hitbox;
        private BoxCollider _chargedAttackHitbox;

        [Header("Effects")]
        [SerializeField] private GameObject _chargingVFX;
        [SerializeField] private GameObject _fullyChargedVFX;
        [SerializeField] private GameObject _hitVFX;
        [SerializeField] private MeshRenderer _weaponMeshRenderer;
        [SerializeField] private Material _chargedAttackMaterial;

        public WeaponAnimationEventHander WeaponAnimationEventHander => _weaponAnimationEventHander;

        private List<Collider> _hitTargetCache;

        private int _currentCombo = 0;
        private bool _hitboxEnabled;
        private bool _chargedAttackHitboxEnabled;
        private float _currentChargedAttackDamage;

        private Material _originalMaterial;

        public bool NextComboInputAllowed { get; private set; }

        //Events
        public event Action OnWeaponHit;

        private void Awake()
        {
            _hitTargetCache = new List<Collider>();
            NextComboInputAllowed = true;

            _chargingVFX.SetActive(false);
            _fullyChargedVFX.SetActive(false);

            _originalMaterial = _weaponMeshRenderer.material;
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
            if (_hitboxEnabled)
            {
                Collider[] colliders = Physics.OverlapBox(_hitbox.transform.position, _hitbox.size * 0.5f, _hitbox.transform.rotation, GameData.DAMAGEABLE_LAYER);
                foreach (var collider in colliders)
                {
                    if (_hitTargetCache.Contains(collider))
                    {
                        continue;
                    }

                    _hitTargetCache.Add(collider);

                    collider.GetComponentInChildren<IDamageable>()?.TakeDamage(_weaponData.AttackDamage);

                    OnWeaponHit?.Invoke();

                    if (_hitVFX != null)
                    {
                        Instantiate(_hitVFX, _hitbox.transform.position, Quaternion.identity);
                    }
                }
            }
            else if (_chargedAttackHitboxEnabled)
            {
                Collider[] colliders = Physics.OverlapBox(_chargedAttackHitbox.transform.position, _hitbox.size * 0.5f, _chargedAttackHitbox.transform.rotation, GameData.DAMAGEABLE_LAYER);
                foreach (var collider in colliders)
                {
                    if (_hitTargetCache.Contains(collider))
                    {
                        continue;
                    }

                    _hitTargetCache.Add(collider);

                    collider.GetComponentInChildren<IDamageable>()?.TakeDamage(_currentChargedAttackDamage);

                    OnWeaponHit?.Invoke();

                    if (_hitVFX != null)
                    {
                        Instantiate(_hitVFX, _chargedAttackHitbox.transform.position, Quaternion.identity);
                    }
                }
            }
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

            
        }

        public void Attack()
        {
            if (!NextComboInputAllowed)
                return;

            _weaponAnimator.SetInteger(GameData.COMBO_HASH, _currentCombo);
            _weaponAnimator.SetTrigger(GameData.ATTACK_HASH);

            NextComboInputAllowed = false;
            _currentCombo = (_currentCombo + 1) % (_weaponData.MaxCombo);
        }

        public void StartCharging()
        {
            _weaponAnimator.SetBool(GameData.ISCHARGING_HASH, true);

            _chargingVFX.SetActive(true);
        }

        public void OnFullyCharged()
        {
            _weaponAnimator.SetTrigger(GameData.FULLYCHARGED_HASH);

            _chargingVFX.SetActive(false);
            _fullyChargedVFX.SetActive(true);

            _weaponMeshRenderer.material = _chargedAttackMaterial;
        }

        public void StopCharging()
        {
            _weaponAnimator.SetBool(GameData.ISCHARGING_HASH, false);

            _chargingVFX.SetActive(false);
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

        public void StartChargedAttack(float chargedPercentage)
        {
            _chargedAttackHitboxEnabled = true;
            _currentChargedAttackDamage = Mathf.Lerp(_weaponData.MinChargedAttackDamage, _weaponData.MaxChargedAttackDamage, chargedPercentage);
        }

        public void StopChargedAttack()
        {
            _chargedAttackHitboxEnabled = false;

            //Reset cache
            _hitTargetCache.Clear();

            _weaponAnimator.SetTrigger(GameData.CHARGEDATTACKENDED_HASH);

            _weaponMeshRenderer.material = _originalMaterial;

            _fullyChargedVFX.SetActive(false);
        }

        public void AllowNextComboInput()
        {
            NextComboInputAllowed = true;
        }

        public void ResetCombo()
        {
            _currentCombo = 0;
        }


        public void SetChargedAttackHitbox(BoxCollider hitbox)
        {
            _chargedAttackHitbox = hitbox;
        }
    }
}