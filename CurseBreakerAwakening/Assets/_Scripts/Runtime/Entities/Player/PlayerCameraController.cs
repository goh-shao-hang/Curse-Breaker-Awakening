using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private Transform _cameraRoot;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _sensitivity;
        [SerializeField] private bool _invertXAxis;
        [SerializeField] private bool _invertYAxis;
        [SerializeField] private float _maxCameraVerticalAngle;

        private float _yaw; //horizontal rotation
        private float _pitch; //vertical rotation

        private void Start()
        {
            Helper.LockAndHideCursor(true);
        }

        private void Update()
        {
            transform.position = _cameraRoot.transform.position;

            _yaw += _playerInputHandler.LookInput.x * _sensitivity;
            _pitch -= _playerInputHandler.LookInput.y * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, -_maxCameraVerticalAngle, _maxCameraVerticalAngle);

            _playerCamera.transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            _cameraRoot.rotation = Quaternion.Euler(0f, _yaw, 0f);
        }
    }
}