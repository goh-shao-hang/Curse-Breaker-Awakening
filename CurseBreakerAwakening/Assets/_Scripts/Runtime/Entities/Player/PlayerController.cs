using CBA.Input;
using GameCells.Modules;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Transform _orientationTransform;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private MovementModule _playerMovementModule;
        [SerializeField] private PhysicsQuery _groundChecker;
        [SerializeField] private PlayerStamina _playerStamina;

        [Header("Camera")]
        [SerializeField] private Transform _cameraManagerTransform;
        [SerializeField] private Transform _cameraFollowTarget;

        [Header(GameData.SETTINGS)]
        [Header("Camera")]
        [SerializeField] private float _xSensitivity = 100f;
        [SerializeField] private float _ySensitivity = 100f;
        [SerializeField] private float _maxCameraVerticalAngle = 80f;
        [SerializeField] private float _cameraSmoothing = 0.1f;

        [Header("Movement")]
        [SerializeField] private float _walkSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 10f;
        [SerializeField] private float _groundDrag = 5f;
        [SerializeField] private float _airMovementMultiplier = 0.4f;
        [SerializeField] private float _maxVelocity = 8f;
        [SerializeField] private bool _sprintConsumeStamina = true;
        [SerializeField] private float _sprintStaminaConsumption = 10f;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 2f;

        private Quaternion _targetCameraRotation;

        private Vector3 _moveDirection;
        private Vector3 _currentHorizontalVelocity;

        private float _camXRotation;
        private float _camYRotation;

        private float _moveSpeed;

        private bool _isSprinting = false;
        private bool _isGrounded => _groundChecker.Hit();

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _moveSpeed = _walkSpeed;
        }

        private void OnEnable()
        {
            _playerInputHandler.OnJumpInput += Jump;
            _playerInputHandler.OnSprintPressedInput += () => Sprint(true);
            _playerInputHandler.OnSprintReleasedInput += () => Sprint(false);
        }

        private void OnDisable()
        {
            _playerInputHandler.OnJumpInput -= Jump;
            _playerInputHandler.OnSprintPressedInput += () => Sprint(true);
            _playerInputHandler.OnSprintReleasedInput += () => Sprint(false);
        }

        private void Update()
        {
            HandleCamera();

            if (_isSprinting && _sprintConsumeStamina)
            {
                CalculateSprintingStamina();
            }
        }

        private void LateUpdate()
        {
            //CameraRotation();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            _playerMovementModule.SetDrag(_isGrounded ? _groundDrag : 0f); 

            _moveDirection = (_orientationTransform.forward * _playerInputHandler.MoveInput.y + _orientationTransform.right * _playerInputHandler.MoveInput.x);

            _playerMovementModule.AddForce(_moveDirection * _moveSpeed * (_isGrounded ? 1 : _airMovementMultiplier), ForceMode.Acceleration);

            /*_currentHorizontalVelocity.Set(_playerMovementModule.CurrentVelocity.x, 0f, _playerMovementModule.CurrentVelocity.z);
            if (_playerMovementModule.CurrentVelocity.magnitude > _maxVelocity)
            {
                Vector3 clampedVelocity = _currentHorizontalVelocity.normalized * _maxVelocity;
                _playerMovementModule.SetVelocity(new Vector3(clampedVelocity.x, _playerMovementModule.CurrentVelocity.y, clampedVelocity.z));
            }*/
        }

        private void HandleCamera()
        {

            _cameraManagerTransform.position = _cameraFollowTarget.position;

            float mouseX = _playerInputHandler.LookInput.x * _xSensitivity;
            float mouseY = _playerInputHandler.LookInput.y * _ySensitivity;

            //Rotate camera
            _camXRotation -= mouseY;
            _camXRotation = Mathf.Clamp(_camXRotation, -_maxCameraVerticalAngle, _maxCameraVerticalAngle);

            _camYRotation += mouseX;

            _targetCameraRotation = Quaternion.Euler(_camXRotation, _camYRotation, 0f);
            _cameraManagerTransform.rotation = _targetCameraRotation;//Quaternion.Lerp(_cameraManagerTransform.rotation, _targetCameraRotation, _cameraSmoothing);

            //Rotate player
            _orientationTransform.Rotate(Vector3.up * mouseX);
        }

        private void Jump()
        {
            if (_isGrounded)
            {
                _playerMovementModule.SetVelocity(new Vector3(_playerMovementModule.CurrentVelocity.x, 0f, _playerMovementModule.CurrentVelocity.z));
                _playerMovementModule.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            }
        }

        private void Sprint(bool isSprinting)
        {
            _isSprinting = isSprinting;
            _moveSpeed = isSprinting ? _sprintSpeed : _walkSpeed;
        }

        public void CalculateSprintingStamina()
        {
            _playerStamina.ConsumeStamina(_sprintStaminaConsumption * Time.deltaTime);

            if (_playerStamina.CurrentStamina <= 0)
            {
                Sprint(false);
            }
        }
    }
}