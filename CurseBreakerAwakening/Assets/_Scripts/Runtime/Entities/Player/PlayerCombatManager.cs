using CBA.Entities.Player.Weapons;
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

        private Weapon _currentWeapon = null;

        private void Start()
        {
            EquipWeapon(_weaponData);
        }

        private void Update()
        {
            if (_playerInputHandler.AttackInput)
            {
                _currentWeapon.Attack();
            }
        }

        private void LateUpdate()
        {
            _weaponHolderTransform.rotation = _playerCameraController.PlayerCamera.transform.rotation;
        }

        private void EquipWeapon(SO_WeaponData weaponData)
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.WeaponAnimationEventHander.OnCameraShakeEvent -= CameraShake;
                Destroy(_currentWeapon.gameObject);
            }

            _currentWeapon = Instantiate(weaponData.WeaponPrefab, _weaponHolderTransform);

            _currentWeapon.WeaponAnimationEventHander.OnCameraShakeEvent += CameraShake;
        }

        public void CameraShake()
        {
            _playerCameraController.CameraShake();
        }
    }
}