using CBA.Core;
using Cinemachine;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace CBA.Entities.Player.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [FormerlySerializedAs("_weaponData")][field: SerializeField] public SO_WeaponData WeaponData;
        [SerializeField] private CombatAnimationEventHander _weaponAnimationEventHander;
        [SerializeField] private Animator _weaponAnimator;
        [SerializeField] private BoxCollider _hitbox;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _attackBufferDuration;

        private PlayerCombatManager _playerCombatManager;
        private BoxCollider _chargedAttackHitbox;

        public bool IsPerformingCombatAction { get; private set; } = false;

        [Header("Effects")]
        [SerializeField] private AudioEmitter _weaponAudioEmitter;
        [SerializeField] private GameObject _chargingVFX;
        [SerializeField] private GameObject _fullyChargedVFX;
        [SerializeField] private GameObject _hitVFX;
        [SerializeField] private VisualEffect _blockSuccessVFX;
        [SerializeField] private VisualEffect _parrySuccessVFX;
        [SerializeField] private MeshRenderer _weaponMeshRenderer;
        [SerializeField] private Material _chargedAttackMaterial;

        public CombatAnimationEventHander WeaponAnimationEventHander => _weaponAnimationEventHander;

        private LayerMask _targetLayers;
        private List<Collider> _hitTargetCache = new List<Collider>();

        private int _currentCombo = 0;
        private bool _hitboxEnabled;
        private bool _chargedAttackHitboxEnabled;
        private bool _normalAttackBuffer;
        private bool _isBlockPressed = false;

        private float _currentBlockTime = 0f;
        private float _currentChargeTime;
        private float _currentChargedAttackDamage;

        private float _blockAndParryAnimationCooldown = 0f;

        private Material _originalMaterial;

        public bool NextComboInputAllowed { get; private set; } = true;

        private Coroutine _normalAttackBufferCO = null;
        private Coroutine _chargingCO = null;
        private Coroutine _chargedAttackCO = null;
        private Coroutine _minBlockTimeCO = null;

        //Events
        public event Action OnWeaponHit;

        public Weapon Initialize(PlayerCombatManager playerCombatManager)
        {
            _playerCombatManager = playerCombatManager;
            _targetLayers = _playerCombatManager.TargetLayers;
            _chargedAttackHitbox = _playerCombatManager.ChargedAttackHitbox;

            _playerCombatManager.PlayerHurtbox.OnParrySuccess.AddListener(OnParrySuccess);
            _playerCombatManager.PlayerHurtbox.OnBlockSuccess.AddListener(OnBlockSuccess);

            return this;
        }

        private void Awake()
        {
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

        private void Update()
        {
            _blockAndParryAnimationCooldown -= Time.deltaTime;

            if (_isBlockPressed)
            {
                _currentBlockTime += Time.deltaTime;
            }
            else if (_normalAttackBuffer)
            {
                Attack();
            }

            if (_hitboxEnabled)
            {
                AttackHitboxDetection();
            }
            else if (_chargedAttackHitboxEnabled)
            {
                ChargedAttackHitboxDetection();
            }
        }

        private void AttackHitboxDetection()
        {
            Collider[] colliders = Physics.OverlapBox(_hitbox.transform.position, _hitbox.size * 0.5f, _hitbox.transform.rotation, _targetLayers);
            foreach (var collider in colliders)
            {
                if (_hitTargetCache.Contains(collider))
                {
                    continue;
                }

                _hitTargetCache.Add(collider);

                collider.GetComponentInChildren<IDamageable>()?.TakeDamage(WeaponData.AttackDamage);

                OnWeaponHit?.Invoke();

                _weaponAudioEmitter?.PlayOneShotSfx("Sword_Hit");

                if (_hitVFX != null)
                {
                    Destroy(Instantiate(_hitVFX, _hitbox.transform.position, Quaternion.identity), 1f);
                }
            }
        }

        private void ChargedAttackHitboxDetection()
        {
            Collider[] colliders = Physics.OverlapBox(_chargedAttackHitbox.transform.position, _chargedAttackHitbox.size * 0.5f, _chargedAttackHitbox.transform.rotation, _targetLayers);
            foreach (var collider in colliders)
            {
                if (_hitTargetCache.Contains(collider))
                {
                    continue;
                }

                _hitTargetCache.Add(collider);

                collider.GetComponentInChildren<IDamageable>()?.TakeDamage(_currentChargedAttackDamage);

                OnWeaponHit?.Invoke();

                _weaponAudioEmitter?.PlayOneShotSfx("Sword_Hit");

                if (_hitVFX != null)
                {
                    Destroy(Instantiate(_hitVFX, _hitbox.transform.position, Quaternion.identity), 1f);
                }
            }
        }

        #region Normal Attack
        public void PrepareAttack()
        {
            _normalAttackBufferCO = StartCoroutine(AttackBufferCO());
        }

        private IEnumerator AttackBufferCO()
        {
            _normalAttackBuffer = true;

            yield return WaitHandler.GetWaitForSeconds(_attackBufferDuration);

            _normalAttackBuffer = false;
        }

        public void Attack()
        {
            if (!NextComboInputAllowed)
                return;

            IsPerformingCombatAction = true;

            _weaponAnimator.SetInteger(GameData.COMBO_HASH, _currentCombo);
            _weaponAnimator.SetTrigger(GameData.ATTACK_HASH);

            _weaponAudioEmitter?.PlayOneShotSfx("Sword_Attack");

            NextComboInputAllowed = false;
            _currentCombo = (_currentCombo + 1) % (WeaponData.MaxCombo);

            _normalAttackBuffer = false;
            StopCoroutine(_normalAttackBufferCO);
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

            IsPerformingCombatAction = false;
        }

        public void ResetCombo()
        {
            _currentCombo = 0;
        }

        #endregion

        #region Charged Attack
        public void PrepareCharge()
        {
            //Don't allow charging if is blocking
            if (_isBlockPressed)
                return;

            _chargingCO = StartCoroutine(ChargingCO());
        }

        public void ReleaseCharge()
        {
            if (_chargingCO == null) //Not charging but attack key released
                return;

            StopCoroutine(_chargingCO);
            _chargingCO = null;

            if (_currentChargeTime < WeaponData.MinChargingTime)
            {
                //Cancelled due to not enough charge time
                return;
            }
            else if (_currentChargeTime >= WeaponData.MinChargingTime)
            {
                //Do Charged Atatck
                PerformChargedAttack(_currentChargeTime - WeaponData.MinChargingTime / WeaponData.MaxChargingTime - WeaponData.MinChargingTime);
            }
        }

        public void InterruptCharging()
        {
            if (_chargingCO == null) //Not charging thus nothing to interrupt
                return;

            StopCoroutine(_chargingCO);
            _chargingCO = null;

            _currentChargeTime = 0f;

            _weaponAnimator.SetBool(GameData.ISCHARGING_HASH, false);

            _weaponMeshRenderer.material = _originalMaterial;
            _chargingVFX.SetActive(false);
            _fullyChargedVFX.SetActive(false);

            IsPerformingCombatAction = false;
        }

        private IEnumerator ChargingCO()
        {
            _currentChargeTime = 0f;

            while (_currentChargeTime < WeaponData.MaxChargingTime)
            {
                _currentChargeTime += Time.deltaTime;

                if (_currentChargeTime >= WeaponData.MinChargingTime)
                {
                    OnChargedAttackReady();
                }

                yield return null;
            }

            //Fully Charged
            _currentChargeTime = WeaponData.MaxChargingTime;
            OnChargedAttackFull();
        }

        public void OnChargedAttackReady()
        {
            IsPerformingCombatAction = true;

            _playerCombatManager.OnChargingStarted?.Invoke();

            _weaponAnimator.SetBool(GameData.ISCHARGING_HASH, true);

            _chargingVFX.SetActive(true);
        }

        public void OnChargedAttackFull()
        {
            _playerCombatManager.OnChargingMaxed?.Invoke();

            _weaponAnimator.SetTrigger(GameData.FULLYCHARGED_HASH);

            _chargingVFX.SetActive(false);
            _fullyChargedVFX.SetActive(true);

            _weaponMeshRenderer.material = _chargedAttackMaterial;
        }

        public void PerformChargedAttack(float chargedPercentage)
        {
            _weaponAnimator.SetTrigger(GameData.CHARGERELEASED_HASH);
            _weaponAnimator.SetBool(GameData.ISCHARGING_HASH, false);
            _chargingVFX.SetActive(false);

            _chargedAttackHitboxEnabled = true;
            _currentChargedAttackDamage = Mathf.Lerp(WeaponData.MinChargedAttackDamage, WeaponData.MaxChargedAttackDamage, chargedPercentage);

            _playerCombatManager.OnChargedAttackReleased?.Invoke(chargedPercentage);
            _chargedAttackCO = StartCoroutine(ChargedAttackCO());
        }

        private IEnumerator ChargedAttackCO()
        {
            float timeElapsed = 0f;

            while (timeElapsed <= WeaponData.ChargedAttackDuration)
            {
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            EndChargedAttack();
        }

        public void EndChargedAttack()
        {
            _playerCombatManager.OnChargedAttackEnded?.Invoke();

            _chargedAttackHitboxEnabled = false;

            //Reset cache
            _hitTargetCache.Clear();

            _weaponAnimator.SetTrigger(GameData.CHARGEDATTACKENDED_HASH);

            _weaponMeshRenderer.material = _originalMaterial;

            _fullyChargedVFX.SetActive(false);

            IsPerformingCombatAction = false;
        }
        #endregion

        #region Blocking
        public void StartBlocking()
        {
            //Cancel charging if is charging
            if (_chargingCO != null)
            {
                InterruptCharging();
            }

            IsPerformingCombatAction = true;

            _isBlockPressed = true;
            _currentBlockTime = 0f;
            _weaponAnimator.SetBool(GameData.ISBlOCKING_HASH, true);


            if (_minBlockTimeCO != null)
            {
                StopCoroutine(_minBlockTimeCO);
            }
        }

        public void StopBlocking()
        {
            if (_currentBlockTime < WeaponData.MinBlockTime)
            {
                StartCoroutine(MinBlockTimeCO());
            }
            else
            {
                _isBlockPressed = false;
                _weaponAnimator.SetBool(GameData.ISBlOCKING_HASH, false);

                IsPerformingCombatAction = false;
            }
        }

        private IEnumerator MinBlockTimeCO()
        {
            yield return WaitHandler.GetWaitForSeconds(WeaponData.MinBlockTime - _currentBlockTime);

            _isBlockPressed = false;
            _weaponAnimator.SetBool(GameData.ISBlOCKING_HASH, false);

            IsPerformingCombatAction = false;
        }

        public void OnParrySuccess()
        {
            _weaponAudioEmitter?.PlayOneShotSfx("Sword_Parry");
            if (_parrySuccessVFX != null)
            {
                _parrySuccessVFX.Play();
            }

            if (_blockAndParryAnimationCooldown > 0f)
                return;

            _blockAndParryAnimationCooldown = 1f;

            _weaponAnimator.SetTrigger(GameData.PARRY_HASH);
        }

        public void OnBlockSuccess()
        {
            _weaponAudioEmitter?.PlayOneShotSfx("Sword_Block");
            if (_blockSuccessVFX != null)
            {
                _blockSuccessVFX.Play();
            }

            if (_blockAndParryAnimationCooldown > 0f)
                return;

            _blockAndParryAnimationCooldown = 1f;

            _weaponAnimator.SetTrigger(GameData.BLOCKSUCCESS_HASH);

        }
        #endregion

    }
}