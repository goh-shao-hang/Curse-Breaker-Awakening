using GameCells.Modules;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Entities.Player
{
    public class FPPlayerController : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Transform _orientationTransform;
        [SerializeField] private MovementModule _movementModule;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private PlayerStamina _playerStamina;
        [SerializeField] private CapsuleCollider _playerCollider;

        public MovementModule MovementModule => _movementModule;

        [Header("Physics Queries")]
        [SerializeField] private PhysicsQuery _groundChecker;
        [SerializeField] private RaycastCheck _slopeChecker;

        [Header(GameData.SETTINGS)]
        [Header("Movement")]
        [SerializeField] private float _walkForce = 50f;
        [SerializeField] private float _sprintForce = 100f;
        [SerializeField] private float _maxSpeed = 8f;
        [SerializeField] private float _groundDrag = 6f;
        [SerializeField] private float _maxSlopeAngle = 40f;

        [Header("Jumping")]
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _airMovementMultiplier = 0.3f;

        [Header("Crouching")]
        [SerializeField] private bool _enableCrouching = true;
        [SerializeField] private float _crouchMoveForce;
        [SerializeField] private float _crouchYScale;

        [Header("Sliding")]
        [SerializeField] private float _slidingForce;

        public UnityEvent OnSprintStart;
        public UnityEvent OnSprintEnd;

        //Input
        private float _horizontalInput;
        private float _verticalInput;
        private bool _isSprintInputHeld;

        //Movement
        private float _currentMoveForce;
        private float _targetMoveForce;
        private Vector3 _moveDirection;
        private Vector3 _currentHorizontalVelocity;

        //Crouch
        private float _initialYScale;
        private bool _isCrouchInputHeld = false;

        //Sliding
        private float _previousTargetMoveForce;

        private bool _isGrounded => _groundChecker.Hit();
        public bool IsOnSlope
        {
            get
            {
                float angle = Vector3.Angle(Vector3.up, _slopeChecker.HitInfo.normal);
                return _slopeChecker.Hit() && angle != 0 && angle < _maxSlopeAngle;
            }
        }

        public bool IsSliding;

        private Vector3 _colliderBottomPoint => _playerCollider.center - (Vector3.up * (_playerCollider.height * 0.5f));

        public enum EMovementState
        {
            WALKING,
            SPRINTING,
            CROUCHING,
            SLIDING,
            AIR,
        }

        private EMovementState _currentMovementState;

        private void Start()
        {
            _initialYScale = transform.localScale.y;
        }

        private void OnEnable()
        {
            _playerInputHandler.OnJumpInput += Jump;
            _playerInputHandler.OnSprintPressedInput += OnSprintPressed;
            _playerInputHandler.OnSprintReleasedInput += OnSprintReleased;
            _playerInputHandler.OnCrouchPressedInput += OnCrouchPressed;
            _playerInputHandler.OnCrouchReleasedInput += OnCrouchReleased;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnJumpInput -= Jump;
            _playerInputHandler.OnSprintPressedInput -= OnSprintPressed;
            _playerInputHandler.OnSprintReleasedInput -= OnSprintReleased;
            _playerInputHandler.OnCrouchPressedInput -= OnCrouchPressed;
            _playerInputHandler.OnCrouchReleasedInput -= OnCrouchReleased;
        }

        private void Update()
        {
            ReadInput();
            ApplyDrag();
            HandleState();
            LimitMaxSpeed();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ReadInput()
        {
            _horizontalInput = _playerInputHandler.MoveInput.x;
            _verticalInput = _playerInputHandler.MoveInput.y;
        }

        private void HandleState()
        {
            //TODO Proper state machine
            if (IsSliding)
            {
                _currentMovementState = EMovementState.SLIDING;

                if (IsOnSlope && _movementModule.CurrentVelocity.y < -0.1f)
                {
                    _targetMoveForce = _slidingForce;
                }
                else
                {
                    _targetMoveForce = _sprintForce;
                }
            }
            else if (_isCrouchInputHeld)
            {
                _currentMovementState = EMovementState.CROUCHING;
                _targetMoveForce = _crouchMoveForce;
            }
            else if (_isGrounded && _isSprintInputHeld && _playerStamina.CurrentStamina > 0)
            {
                _currentMovementState = EMovementState.SPRINTING;
                _targetMoveForce = _sprintForce;
                //_movementModule.SetDrag(_groundDrag);
            }
            else if (_isGrounded)
            {
                _currentMovementState = EMovementState.WALKING;
                _targetMoveForce = _walkForce;
                //_movementModule.SetDrag(_groundDrag);
            }
            else
            {
                _currentMovementState = EMovementState.AIR;
                //_movementModule.SetDrag(0);
            }

            if (Mathf.Abs(_targetMoveForce - _previousTargetMoveForce) > 40f && _currentMoveForce != 0)
            {
                StopAllCoroutines();
                StartCoroutine(LerpMoveSpeed());
            }
            else
            {
                _currentMoveForce = _targetMoveForce;
            }

            _previousTargetMoveForce = _targetMoveForce;
        }

        private void ApplyDrag()
        {
            _movementModule.SetDrag(_isGrounded ? _groundDrag : 0);
        }

        private void ApplyMovement()
        {
            _moveDirection = (_orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput);

            //Project move direction to slope if on slope
            if (IsOnSlope)
            {
                _moveDirection = ProjectDirectionOnSlope(_moveDirection);

                if (_playerInputHandler.MoveInput == Vector2.zero)
                {
                    _movementModule.SetUseGravity(false);
                }
                else
                {
                    _movementModule.SetUseGravity(true);
                }
            }
            else
            {
                _movementModule.SetUseGravity(true);
            }

            _movementModule.AddForce(_moveDirection * _currentMoveForce * (_isGrounded ? 1 : _airMovementMultiplier), ForceMode.Acceleration);
        }

        public Vector3 ProjectDirectionOnSlope(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, _slopeChecker.HitInfo.normal).normalized;
        }

        private void LimitMaxSpeed()
        {
            if (!IsOnSlope)
            {
                _currentHorizontalVelocity.Set(_movementModule.CurrentVelocity.x, 0f, _movementModule.CurrentVelocity.z);
                if (_currentHorizontalVelocity.magnitude > _maxSpeed)
                {
                    Vector3 clampedVelocity = _currentHorizontalVelocity.normalized * _maxSpeed;
                    _movementModule.SetVelocity(new Vector3(clampedVelocity.x, _movementModule.CurrentVelocity.y, clampedVelocity.z));
                }
            }
            else
            {
                //Different approach of limiting velocity when on slope
                if (_movementModule.CurrentVelocity.magnitude > _maxSpeed)
                {
                    _movementModule.SetVelocity(_movementModule.CurrentVelocity.normalized * _maxSpeed);
                }
            }
        }

        private void Jump()
        {
            if (!_isGrounded)
                return;

            //Reset y velocity
            _movementModule.SetVelocity(new Vector3(_movementModule.CurrentVelocity.x, 0f, _movementModule.CurrentVelocity.z));

            _movementModule.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        private void OnSprintPressed()
        {
            _isSprintInputHeld = true;

            OnSprintStart?.Invoke();
        }

        private void OnSprintReleased()
        {
            _isSprintInputHeld = false;

            OnSprintEnd?.Invoke();
        }

        private void OnCrouchPressed()
        {
            _isCrouchInputHeld = true;

            _groundChecker.SetOffset(_colliderBottomPoint);

            transform.localScale = new Vector3(transform.localScale.x, _crouchYScale, transform.localScale.z);

            _movementModule.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        private void OnCrouchReleased()
        {
            _isCrouchInputHeld = false;

            _groundChecker.SetOffset(_colliderBottomPoint);

            transform.localScale = new Vector3(transform.localScale.x, _initialYScale, transform.localScale.z);
        }

        private IEnumerator LerpMoveSpeed()
        {
            float timeElapsed = 0f;
            float speedDifference = Mathf.Abs(_targetMoveForce - _currentMoveForce);
            float startValue = _currentMoveForce;

            while (timeElapsed < speedDifference)
            {
                _currentMoveForce = Mathf.Lerp(startValue, _targetMoveForce, timeElapsed / speedDifference);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _currentMoveForce = _targetMoveForce;
        }
    }
}