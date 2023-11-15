using CBA.Entities.Player.Weapons;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField] private Weapon _currentWeapon = null;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _attackBufferDuration = 0.2f;

        public bool AttackBuffer { get; private set; } = false;

        private Coroutine _attackBufferCO = null;

        private void OnEnable()
        {
            _playerInputHandler.OnAttackPressedInput += OnAttackPressed;

            _currentWeapon.WeaponAnimationEventHander.OnCameraShakeEvent += CameraShake;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnAttackPressedInput -= OnAttackPressed;

            _currentWeapon.WeaponAnimationEventHander.OnCameraShakeEvent -= CameraShake;
        }

        private void Update()
        {
            if (AttackBuffer)
            {
                _currentWeapon.Attack();
            }
        }

        private void LateUpdate()
        {
            _weaponHolderTransform.rotation = _playerCameraController.PlayerCamera.transform.rotation;
        }

        private void OnAttackPressed()
        {
            AttackBuffer = true;

            if (_attackBufferCO != null)
            {
                StopCoroutine(_attackBufferCO);
            }

            _attackBufferCO = StartCoroutine(ResetAttackBufferCO());
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

        /*private void EquipWeapon(SO_WeaponData weaponData)
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.WeaponAnimationEventHander.OnCameraShakeEvent -= CameraShake;
                Destroy(_currentWeapon.gameObject);
            }

            _currentWeapon = Instantiate(weaponData.WeaponPrefab, _weaponHolderTransform);

            _currentWeapon.WeaponAnimationEventHander.OnCameraShakeEvent += CameraShake;
        }*/

        public void CameraShake(int direction)
        {
            _playerCameraController.CameraShake(direction);
        }
    }
}