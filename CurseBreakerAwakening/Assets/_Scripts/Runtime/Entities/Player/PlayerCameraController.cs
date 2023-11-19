using Cinemachine;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CBA.Entities.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
        public Camera PlayerCamera => _playerCamera;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _sensitivity;
        [SerializeField] private bool _invertXAxis;
        [SerializeField] private bool _invertYAxis;
        [SerializeField] private float _maxCameraVerticalAngle;
        public bool _cameraMovementLocked { get; private set; }

        private float _yaw; //horizontal rotation
        private float _pitch; //vertical rotation
        private float _tiltRotation = 0f;

        private void Awake()
        {
            //TODO
            LeanTween.reset();
        }

        private void Start()
        {
            Helper.LockAndHideCursor(true);
        }

        private void OnEnable()
        {
            _playerController.OnWallRunStarted += TiltCamera;
            _playerController.OnWallRunEnded += StopCameraTilt;
            _playerController.PlayerCombatManager.OnChargedAttackReleased += (x) => LockCameraMovement(true);
            _playerController.PlayerCombatManager.OnChargedAttackEnded += () => LockCameraMovement(false);
        }

        private void OnDisable()
        {
            _playerController.OnWallRunStarted -= TiltCamera;
            _playerController.OnWallRunEnded -= StopCameraTilt;
            _playerController.PlayerCombatManager.OnChargedAttackReleased -= (x) => LockCameraMovement(true);
            _playerController.PlayerCombatManager.OnChargedAttackEnded -= () => LockCameraMovement(false);
        }

        private void Update()
        {
            if (!_cameraMovementLocked)
            {
                _yaw += _playerController.PlayerInputHandler.LookInput.x * _sensitivity;
                _pitch -= _playerController.PlayerInputHandler.LookInput.y * _sensitivity;
                _pitch = Mathf.Clamp(_pitch, -_maxCameraVerticalAngle, _maxCameraVerticalAngle);
                transform.rotation = Quaternion.Euler(_pitch, _yaw, _tiltRotation);
                _playerController.CameraRootTransform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            }

            transform.position = _playerController.CameraRootTransform.transform.position;
        }

        public void LockCameraMovement(bool locked)
        {
            Debug.Log("LOCK");
            _cameraMovementLocked = locked;
        }

        public void CameraShake(Vector3 direction, float strength = 0.3f)
        {
            //_cinemachineImpulseSource.GenerateImpulse(strength);
            _cinemachineImpulseSource.GenerateImpulseWithVelocity(direction * strength);

        }

        public void TiltCamera(bool tiltRight)
        {
            LeanTween.cancelAll(gameObject);

            if (tiltRight)
                LeanTween.value(gameObject, _tiltRotation, _playerController.WallRunCameraTilt, 0.2f).setOnUpdate(value => _tiltRotation = value);
            else
                LeanTween.value(gameObject, _tiltRotation, -_playerController.WallRunCameraTilt, 0.2f).setOnUpdate(value => _tiltRotation = value);
        }

        public void StopCameraTilt()
        {
            LeanTween.cancelAll(gameObject);

            LeanTween.value(gameObject, _tiltRotation, 0f, 0.2f).setOnUpdate(value => _tiltRotation = value);
        }
    }
}