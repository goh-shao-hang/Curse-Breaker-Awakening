using CBA.Core;
using CBA.Entities.Player.Weapons;
using GameCells;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CBA.Entities.Player
{
    public class PlayerCombatManager : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField] private SO_WeaponData _weaponData;

        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private PlayerCameraController _playerCameraController;
        [SerializeField] private PlayerGrabManager _playerGrabManager;
        [SerializeField] private Transform _weaponHolderTransform;
        [field: SerializeField] public BoxCollider ChargedAttackHitbox;
        [field: SerializeField] public PlayerHurtbox PlayerHurtbox;

        [field: SerializeField] public Weapon CurrentWeapon = null;

        [Header(GameData.SETTINGS)]
        [field: SerializeField] public float BlockStaminaConsumption = 20f;
        [field: SerializeField] public LayerMask TargetLayers;
        [SerializeField] private float _attackBufferDuration = 0.2f;

        [Header("DEBUG")]
        [SerializeField] private float _duration;
        [SerializeField] private float _strength;
        [SerializeField] private float _vibrato;
        [SerializeField] private bool _equipWeapon = true;

        public bool AttackBuffer { get; private set; } = false;
        public bool CanPerformCombatActions => !_playerGrabManager.IsGrabbing;

        public UnityEvent OnPlayerWeaponHit; //Trigger things like camera shakes

        public Action OnChargingStarted;
        public Action OnChargingMaxed;
        public Action<float> OnChargedAttackReleased;
        public Action OnChargedAttackEnded;

        private void Start()
        {
            if (_equipWeapon)
            {
                EquipWeapon(_weaponData);
            }
        }

        private void LateUpdate()
        {
            //Weapon rotation
            _weaponHolderTransform.rotation = _playerCameraController.PlayerCamera.transform.rotation;
            ChargedAttackHitbox.transform.rotation = Quaternion.Euler(0f, _playerCameraController.PlayerCamera.transform.eulerAngles.y, 0f);
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

        private void OnAttackPressed()
        {
            if (!CanPerformCombatActions)
                return;

            CurrentWeapon.PrepareAttack();
            CurrentWeapon.PrepareCharge();
        }

        private void OnAttackReleased()
        {
            CurrentWeapon.ReleaseCharge();
        }

        private void OnBlockPressed()
        {
            if (!CanPerformCombatActions)
                return;

            CurrentWeapon.StartBlocking();
        }

        private void OnBlockReleased()
        {
            CurrentWeapon.StopBlocking();
        }

        public void InterruptBlocking()
        {
            CurrentWeapon.StopBlocking();
        }

        private void EquipWeapon(SO_WeaponData weaponData)
        {
            if (CurrentWeapon != null)
            {
                //Unsubscribe from events
                CurrentWeapon.OnWeaponHit -= TriggerOnPlayerWeaponHit;
                    
                Destroy(CurrentWeapon.gameObject);
            }
            CurrentWeapon = Instantiate(weaponData.WeaponPrefab, _weaponHolderTransform).Initialize(this);
            //Subscribe to events
            CurrentWeapon.OnWeaponHit += TriggerOnPlayerWeaponHit;
        }

        private void TriggerOnPlayerWeaponHit()
        {
            GameEventsManager.Instance.CameraShake(Vector3.one, 0.3f);
        }
    }
}