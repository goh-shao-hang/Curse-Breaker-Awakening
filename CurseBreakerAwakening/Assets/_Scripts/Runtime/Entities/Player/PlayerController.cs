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
        [SerializeField] private CameraManager _playerCameraManager;
        [SerializeField] private MovementModule _playerMovementModule;
        [SerializeField] private PhysicsQuery _groundChecker;
        [SerializeField] private RaycastCheck _slopeChecker;
        [SerializeField] private PlayerStamina _playerStamina;

        [Header("Camera")]
        [SerializeField] private Transform _cameraFollowTarget;
        [SerializeField] private float _normalFieldOfView = 60f;
        [SerializeField] private float _sprintingFieldOfView = 75f;

        [Header(GameData.SETTINGS)]
        [Header("Camera")]
        [SerializeField] private float _xSensitivity = 100f;
        [SerializeField] private float _ySensitivity = 100f;
        [SerializeField] private float _maxCameraVerticalAngle = 80f;

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

        private bool _isSprintInputHeld;
        private bool _isSprinting = false;
        private bool _isGrounded => _groundChecker.Hit();
        private bool _isOnSlope => _slopeChecker.Hit() && _slopeChecker.HitInfo.normal != Vector3.up;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _moveSpeed = _walkSpeed;
        }

        private void OnEnable()
        {
            _playerInputHandler.OnJumpInput += Jump;
            _playerInputHandler.OnSprintPressedInput += OnSprintPressed;
            _playerInputHandler.OnSprintReleasedInput += OnSprintReleased;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnJumpInput -= Jump;
            _playerInputHandler.OnSprintPressedInput -= OnSprintPressed;
            _playerInputHandler.OnSprintReleasedInput -= OnSprintReleased;
        }

        private void Update()
        {
            HandleCamera();

            HandleSprinting();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            #region 1. Drag Control
            //1. Set drag depends on grounded state
            _playerMovementModule.SetDrag(_isGrounded ? _groundDrag : 0f);
            #endregion

            #region 2. Movement Calculation
            //2. Calculate move direction, project direction if on slope
            _moveDirection = (_orientationTransform.forward * _playerInputHandler.MoveInput.y + _orientationTransform.right * _playerInputHandler.MoveInput.x).normalized;

            if (_isOnSlope) 
            {
                //Project move direction to plane if on slope
                _moveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeChecker.HitInfo.normal).normalized;

                //If player is not moving, disable gravity to prevent sliding
                _playerMovementModule.SetUseGravity(_playerInputHandler.MoveInput != Vector2.zero);
            }
            else
            {
                _playerMovementModule.SetUseGravity(true);
            }

            _playerMovementModule.AddForce(_moveDirection * _moveSpeed * (_isGrounded ? 1 : _airMovementMultiplier), ForceMode.Acceleration);
            #endregion

            #region 3. Limit Max Speed
            //3. Limiting max speed
            if (!_isOnSlope)
            {
                _currentHorizontalVelocity.Set(_playerMovementModule.CurrentVelocity.x, 0f, _playerMovementModule.CurrentVelocity.z);
                if (_playerMovementModule.CurrentVelocity.magnitude > _maxVelocity)
                {
                    Vector3 clampedVelocity = _currentHorizontalVelocity.normalized * _maxVelocity;
                    _playerMovementModule.SetVelocity(new Vector3(clampedVelocity.x, _playerMovementModule.CurrentVelocity.y, clampedVelocity.z));
                }
            }
            else
            {
                //Different approach of limiting velocity when on slope
                if (_playerMovementModule.CurrentVelocity.magnitude > _maxVelocity)
                {
                    _playerMovementModule.SetVelocity(_playerMovementModule.CurrentVelocity.normalized * _maxVelocity);
                }
            }
            #endregion
        }

        private void HandleCamera()
        {
            //Update camera position since it is not parented to the player. This avoids camera jitter due to rigidbody calculations
            _playerCameraManager.transform.position = _cameraFollowTarget.position;

            float mouseX = _playerInputHandler.LookInput.x * _xSensitivity;
            float mouseY = _playerInputHandler.LookInput.y * _ySensitivity;

            //Rotate camera
            _camXRotation -= mouseY;
            _camXRotation = Mathf.Clamp(_camXRotation, -_maxCameraVerticalAngle, _maxCameraVerticalAngle);

            _camYRotation += mouseX;

            _targetCameraRotation = Quaternion.Euler(_camXRotation, _camYRotation, 0f);
            _playerCameraManager.transform.rotation = _targetCameraRotation;

            //Rotate player
            _orientationTransform.Rotate(Vector3.up * mouseX);
        }

        private void Jump()
        {
            if (_isGrounded)
            {
                //Reset y velocity
                _playerMovementModule.SetVelocity(new Vector3(_playerMovementModule.CurrentVelocity.x, 0f, _playerMovementModule.CurrentVelocity.z));

                //Add upwards force
                _playerMovementModule.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            }
        }

        private void HandleSprinting()
        {
            //If sprint is pressed and is moving, start sprinting
            if (_isSprintInputHeld && _playerInputHandler.MoveInput != Vector2.zero && !_isSprinting && _playerStamina.CurrentStamina > 0)
            {
                StartSprinting();
            }
            //If either button is released or if player not moving, stop sprinting
            else if (_isSprinting && (!_isSprintInputHeld || _playerInputHandler.MoveInput == Vector2.zero))
            {
                StopSprinting();
            }

            //Stamina calculation
            if (_isSprinting && _sprintConsumeStamina)
            {
                CalculateSprintingStaminaConsumption();
            }

        }

        private void OnSprintPressed()
        {
            _isSprintInputHeld = true;
        }

        private void OnSprintReleased()
        {
            _isSprintInputHeld = false;
        }

        private void StartSprinting()
        {
            _isSprinting = true;
            _moveSpeed = _sprintSpeed;

            _playerCameraManager.SetFieldOfView(_sprintingFieldOfView);
        }

        private void StopSprinting()
        {
            _isSprinting = false;
            _moveSpeed = _walkSpeed;

            _playerCameraManager.SetFieldOfView(_normalFieldOfView);
        }

        public void CalculateSprintingStaminaConsumption()
        {
            _playerStamina.ConsumeStamina(_sprintStaminaConsumption * Time.deltaTime);

            if (_playerStamina.CurrentStamina <= 0)
            {
                StopSprinting();
            }
        }
    }
}