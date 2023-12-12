using CBA.Entities.Player.Weapons;
using GameCells;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Entities.Player
{
    public class PlayerCombatManager : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField] private SO_WeaponData _weaponData;

        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private PlayerCameraController _playerCameraController;
        [SerializeField] private Transform _weaponHolderTransform;
        [field: SerializeField] public BoxCollider ChargedAttackHitbox;

        [SerializeField] private Weapon _currentWeapon = null;

        [Header(GameData.SETTINGS)]
        [field: SerializeField] public float BlockStaminaConsumption = 20f;
        [field: SerializeField] public LayerMask TargetLayers;
        [SerializeField] private float _attackBufferDuration = 0.2f;

        [Header("Charged Attack")]
        [SerializeField] private float _chargedAttackDuration = 1f;
        [SerializeField] private float _minChargeTime = 0.5f;
        [SerializeField] private float _maxChargeTime = 2f;

        [Header("DEBUG")]
        [SerializeField] private float _duration;
        [SerializeField] private float _strength;
        [SerializeField] private float _vibrato;
        [SerializeField] private bool _equipWeapon = true;

        public bool AttackBuffer { get; private set; } = false;
        public bool IsBlocking { get; private set; } = false;


        public UnityEvent OnPlayerWeaponHit; //Trigger things like camera shakes

        public event Action OnChargingStarted;
        public event Action OnChargingMaxed;
        public event Action<float> OnChargedAttackReleased;
        public event Action OnChargedAttackEnded;

        private Coroutine _attackBufferCO = null;
        private Coroutine _chargingCO = null;
        private Coroutine _chargedAttackCO = null;

        private float _currentChargeTime = 0f;

        private void Start()
        {
            if (_equipWeapon)
            {
                EquipWeapon(_weaponData);
            }
        }

        private void OnEnable()
        {
            _playerInputHandler.OnAttackPressedInput += OnAttackPressed;
            _playerInputHandler.OnAttackReleasedInput += OnAttackReleased;

            _playerInputHandler.OnBlockPressedInput += OnBlockPressed;
            _playerInputHandler.OnBlockReleasedInput += OnBlockReleased;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnAttackPressedInput -= OnAttackPressed;
            _playerInputHandler.OnAttackReleasedInput -= OnAttackReleased;

            _playerInputHandler.OnBlockPressedInput -= OnBlockPressed;
            _playerInputHandler.OnBlockReleasedInput -= OnBlockReleased;
        }

        private void Update()
        {
            if (AttackBuffer && _currentWeapon.NextComboInputAllowed && _chargedAttackCO == null) //if _chargedAttackCO is not null, charged attack is in progress
            {
                ConsumeAttackBuffer();
                _currentWeapon.Attack();
            }
        }

        private void LateUpdate()
        {
            //Weapon rotation
            _weaponHolderTransform.rotation = _playerCameraController.PlayerCamera.transform.rotation;
            ChargedAttackHitbox.transform.rotation = Quaternion.Euler(0f, _playerCameraController.PlayerCamera.transform.eulerAngles.y, 0f);
        }

        private void EquipWeapon(SO_WeaponData weaponData)
        {
            if (_currentWeapon != null)
            {
                //Unsubscribe from events
                _currentWeapon.OnWeaponHit -= TriggerOnPlayerWeaponHit;
                Destroy(_currentWeapon.gameObject);
            }

            _currentWeapon = Instantiate(weaponData.WeaponPrefab, _weaponHolderTransform).Initialize(this);

            //Subscribe to events
            _currentWeapon.OnWeaponHit += TriggerOnPlayerWeaponHit;
        }

        private void OnAttackPressed()
        {
            AttackBuffer = true;

            if (_attackBufferCO != null)
            {
                StopCoroutine(_attackBufferCO);
            }

            _attackBufferCO = StartCoroutine(ResetAttackBufferCO());
            _chargingCO = StartCoroutine(ChargingCO());
        }

        private void OnAttackReleased()
        {
            if (_chargingCO != null) 
            {
                StopCoroutine(_chargingCO);
            }

            if (_currentChargeTime >= _minChargeTime)
            {
                //Release Charged Attack
                //Broadcast percentage charged
                OnChargedAttackReleased?.Invoke(_currentChargeTime - _minChargeTime / _maxChargeTime - _minChargeTime);

                _chargedAttackCO = StartCoroutine(ChargedAttackCO());
                _currentWeapon.StopCharging();
            }

            _currentChargeTime = 0f;
        }

        public void ConsumeAttackBuffer()
        {
            AttackBuffer = false;

            if (_attackBufferCO != null)
            {
                StopCoroutine(_attackBufferCO);
                _attackBufferCO = null;
            }
        }

        private IEnumerator ResetAttackBufferCO()
        {
            yield return WaitHandler.GetWaitForSeconds(_attackBufferDuration);

            ConsumeAttackBuffer();
        }

        private void OnBlockPressed()
        {
            StartBlocking();
        }

        private void OnBlockReleased()
        {
            StopBlocking();
        }

        public void StartBlocking()
        {
            _currentWeapon.StartBlocking();
        }

        public void StopBlocking()
        {
            _currentWeapon.StopBlocking();
        }

        public void SetIsBlocking(bool isBlocking)
        {
            IsBlocking = isBlocking;
        }

        private IEnumerator ChargingCO()
        {
            _currentChargeTime = 0f;

            while (_currentChargeTime <= _maxChargeTime)
            {
                _currentChargeTime += Time.deltaTime;

                //Charging is only valid upon reaching the min charge time
                if (_currentChargeTime >= _minChargeTime)
                {
                    _currentWeapon.StartCharging();
                    OnChargingStarted?.Invoke();
                }

                yield return null;
            }

            _currentChargeTime = _maxChargeTime;
            _currentWeapon.OnFullyCharged();
            OnChargingMaxed?.Invoke();

            _chargingCO = null;
        }

        private IEnumerator ChargedAttackCO()
        {
            float timeElapsed = 0f;

            _currentWeapon.StartChargedAttack(_currentChargeTime - _minChargeTime / _maxChargeTime - _minChargeTime);

            while (timeElapsed <= _chargedAttackDuration)
            {
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            _currentWeapon.StopChargedAttack();
            OnChargedAttackEnded?.Invoke();

            _chargedAttackCO = null;
        }

        

        private void TriggerOnPlayerWeaponHit()
        {
            _playerCameraController.CameraShakeOmni(_duration, _strength, _vibrato);
            OnPlayerWeaponHit?.Invoke();
        }
    }
}