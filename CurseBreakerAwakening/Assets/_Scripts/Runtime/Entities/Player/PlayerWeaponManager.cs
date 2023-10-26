using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerWeaponManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Transform _weaponHolderTransform;
        [SerializeField] private Camera _playerCamera;

        private void LateUpdate()
        {
            _weaponHolderTransform.transform.SetPositionAndRotation(_playerCamera.transform.position, _playerCamera.transform.rotation);
        }
    }
}