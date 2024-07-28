using CBA.Core;
using Cinemachine;
using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace CBA.Entities.Player
{
    public class PlayerCameraController : MonoBehaviour
    {

        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private GameEventsManager gameEventsManager;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;
        public Camera PlayerCamera => _playerCamera;

        [Header(GameData.SETTINGS)]
        [Range(1, 10)][SerializeField] private float _mouseSensitivity;
        [Range(1, 10)][SerializeField] private float _controllerSensitivity;
        [SerializeField] private bool _invertXAxis;
        [SerializeField] private bool _invertYAxis;
        [SerializeField] private float _maxCameraVerticalAngle;
        public bool _cameraMovementLocked { get; private set; }

        private float _yaw; //horizontal rotation
        private float _pitch; //vertical rotation
        private float _tiltRotation = 0f;

        private void Start()
        {
            Helper.LockAndHideCursor(true);
        }

        private void OnEnable()
        {
            if (SettingsManager.Instance != null)
            {
                SetMouseSensitivity();
                SettingsManager.Instance.OnMouseSensitivityChanged += SetMouseSensitivity;

                SetControllerSensitivity();
                SettingsManager.Instance.OnControllerSensitivityChanged += SetControllerSensitivity;
            }

            if (GameEventsManager.Instance != null)
            {
                GameEventsManager.Instance.OnCameraShakeEvent += CameraShake;
            }

            _playerController.OnWallRunStarted += TiltCamera;
            _playerController.OnWallRunEnded += StopCameraTilt;
            _playerController.PlayerCombatManager.OnChargedAttackReleased += (x) => LockCameraMovement(true);
            _playerController.PlayerCombatManager.OnChargedAttackEnded += () => LockCameraMovement(false);
        }

        private void OnDisable()
        {
            if (SettingsManager.Instance != null)
            {
                SettingsManager.Instance.OnMouseSensitivityChanged -= SetMouseSensitivity;
                SettingsManager.Instance.OnControllerSensitivityChanged -= SetControllerSensitivity;
            }

            if (GameEventsManager.Instance != null)
            {
                GameEventsManager.Instance.OnCameraShakeEvent -= CameraShake;
            }

            _playerController.OnWallRunStarted -= TiltCamera;
            _playerController.OnWallRunEnded -= StopCameraTilt;
            _playerController.PlayerCombatManager.OnChargedAttackReleased -= (x) => LockCameraMovement(true);
            _playerController.PlayerCombatManager.OnChargedAttackEnded -= () => LockCameraMovement(false);
        }

        private void Update()
        {
            //TODO extremely hacky fix
            bool isUsingGamepad = Gamepad.current != null && Gamepad.current.rightStick.IsActuated();
            float sensitivity = isUsingGamepad ? _controllerSensitivity : _mouseSensitivity;

            if (!_cameraMovementLocked)
            {
                _yaw += _playerController.PlayerInputHandler.LookInput.x * sensitivity;
                _pitch -= _playerController.PlayerInputHandler.LookInput.y * sensitivity;
                _pitch = Mathf.Clamp(_pitch, -_maxCameraVerticalAngle, _maxCameraVerticalAngle);
                _playerController.CameraRootTransform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            }

            transform.SetPositionAndRotation(_playerController.CameraRootTransform.transform.position, Quaternion.Euler(_pitch, _yaw, _tiltRotation));
        }

        private void SetMouseSensitivity()
        {
            float sensitivity = SettingsManager.Instance.GetMouseSensitivity();
            if (sensitivity != -1) //Unset
            {
                _mouseSensitivity = sensitivity / 10;
                _mouseSensitivity = Mathf.Lerp(0.05f, 0.5f, _mouseSensitivity);
            }
        }

        private void SetControllerSensitivity()
        {
            float sensitivity = SettingsManager.Instance.GetControllerSensitivity();
            if (sensitivity != -1) //Unset
            {
                _controllerSensitivity = sensitivity / 10;
                _controllerSensitivity = Mathf.Lerp(0.5f, 5f, _controllerSensitivity);
            }
        }

        public void ResetCameraRotation()
        {
            _yaw = 0f;
            _pitch = 0f;
        }

        public void SetCameraRotation(float yaw, float pitch)
        {
            _yaw = yaw;
            _pitch = pitch;
        }

        public void LockCameraMovement(bool locked)
        {
            _cameraMovementLocked = locked;
        }

        public void CameraShake(Vector3 direction, float strength = 0.3f)
        {
            _cinemachineImpulseSource.GenerateImpulseWithVelocity(direction * strength);
        }

        private Tween _cameraShakeTween = null;
        public void CameraShakeOmni(float duration = 0.15f, float strength = .75f, float vibrato = 10f)
        {
            //StartCoroutine(CameraShake(duration, strength));
            if (_cameraShakeTween != null && _cameraShakeTween.IsPlaying())
                return;

            _cameraShakeTween = _playerVirtualCamera.transform.DOShakePosition(duration, strength, 10).OnComplete(() => _cameraShakeTween = null);
        }

        public IEnumerator CameraShake(float duration, float magnitude)
        {
            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                float xOffset = Random.Range(-1f, 1f) * magnitude;
                float yOffset = Random.Range(-1f, 1f) * magnitude;
                _playerVirtualCamera.transform.localPosition = Vector3.Lerp(_playerVirtualCamera.transform.localPosition, new Vector3(xOffset, yOffset, _playerVirtualCamera.transform.localPosition.z), 0.1f);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _playerVirtualCamera.transform.localPosition = Vector3.zero;
        }

        public void TiltCamera(bool tiltRight)
        {
            LeanTween.cancelAll(gameObject);

            if (tiltRight)
                LeanTween.value(gameObject, _tiltRotation, _playerController.WallRunCameraTilt, 0.5f).setEase(LeanTweenType.easeOutBack).setOnUpdate(value => _tiltRotation = value);
            else
                LeanTween.value(gameObject, _tiltRotation, -_playerController.WallRunCameraTilt, 0.5f).setEase(LeanTweenType.easeOutBack).setOnUpdate(value => _tiltRotation = value);
        }

        public void StopCameraTilt()
        {
            LeanTween.cancelAll(gameObject);

            LeanTween.value(gameObject, _tiltRotation, 0f, 0.2f).setEase(LeanTweenType.linear).setOnUpdate(value => _tiltRotation = value);
        }
    }
}