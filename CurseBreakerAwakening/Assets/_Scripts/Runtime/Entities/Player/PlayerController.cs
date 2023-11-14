using GameCells.Modules;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerController : StateManager<EPlayerMovementState>
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private Transform _cameraRootTransform;
        [SerializeField] private MovementModule _movementModule;
        [SerializeField] private CapsuleCollider _playerCollider;
        [SerializeField] private PhysicsQuery _groundChecker;
        [SerializeField] private RaycastCheck _slopeChecker;

        [Header("Movement Settings")]
        [SerializeField] private float _walkMovementForce = 50f;
        [SerializeField] private float _sprintMovementForce = 100f;
        [SerializeField] private float _crouchMovementForce = 30f;
        [SerializeField] private float _maxSpeed = 15f;
        [SerializeField] private float _groundDrag = 7f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _jumpBufferDuration = 0.2f;
        [SerializeField] private float _airMovementMultiplier = 0.1f;
        [SerializeField] private float _maxSlopeAngle = 45f;

        [Header("Stamina Settings")]
        [SerializeField] private float _maxStamina = 100f;
        [SerializeField] private float _staminaRegenRate = 20f;
        [SerializeField] private float _sprintingStaminaConsumption = 10f;
        [SerializeField] private float _staminaRegenDelay = 1f;

        [Header("Wall Run")]
        [SerializeField] private bool _canWallRun = true;
        [SerializeField] private float _wallRunTime = 5f;
        [SerializeField] private float _wallRunMovementForce = 50f;
        [SerializeField] private float _wallCheckDistance = 0.5f;
        [SerializeField] private float _minWallRunHeight;
        [SerializeField] private float _wallRunGravity = 3f;
        [SerializeField] private float _wallRunCameraTilt = 10f;

        #region Getters
        public PlayerInputHandler PlayerInputHandler => _playerInputHandler;
        public MovementModule MovementModule => _movementModule;
        public CapsuleCollider PlayerCollider => _playerCollider;
        public PhysicsQuery GroundChecker => _groundChecker;
        public Transform CameraRootTransform => _cameraRootTransform;

        public float WalkMovementForce => _walkMovementForce;
        public float SprintMovementForce => _sprintMovementForce;
        public float CrouchMovementForce => _crouchMovementForce;
        public float WallRunMovementForce => _wallRunMovementForce;
        public float MaxStamina => _maxStamina;
        public float CurrentStamina { get; private set; }
        public float SpritingStaminaConsumption => _sprintingStaminaConsumption;
        public float WallRunGravity => _wallRunGravity;
        public float WallRunCameraTilt => _wallRunCameraTilt;
        public bool CanWallRun => _canWallRun;

        public bool IsGrounded => _groundChecker.Hit();
        public bool IsOnSlope
        {
            get
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeChecker.HitInfo.normal);
                return _slopeChecker.Hit() && slopeAngle != 0 && slopeAngle < _maxSlopeAngle;
            }
        }

        public bool IsRunnableWallDetected
        {
            get
            {
                if (Physics.Raycast(transform.position, Vector3.down, _minWallRunHeight, GameData.TERRAIN_LAYER))
                {
                    return false;
                }

                if (Physics.Raycast(transform.position, _cameraRootTransform.right, out runnableWallHitInfo, _wallCheckDistance, GameData.TERRAIN_LAYER))
                {
                    return true;
                }
                else if (Physics.Raycast(transform.position, -_cameraRootTransform.right, out runnableWallHitInfo, _wallCheckDistance, GameData.TERRAIN_LAYER))
                {
                    return true;
                }

                return false;
            }
        }
        public RaycastHit runnableWallHitInfo;

        public Vector3 ColliderBottomPoint => _playerCollider.center - (Vector3.up * (_playerCollider.height * 0.5f));
        #endregion

        #region Variables
        public bool _jumpBuffer { get; private set; } = false;
        private Coroutine _jumpBufferCO = null;

        private float _movementForce;
        private float _staminaRegenTimer;
        private Vector3 _moveDirection;
        private Vector3 _horizontalVelocity;
        #endregion

        #region Events
        public event Action<float> OnStaminaChanged;
        public event Action<bool> OnWallRunStarted;
        public event Action OnWallRunEnded;
        #endregion


        protected override void Awake()
        {
            base.Awake();

            //State Machine Initialization
            _StatesDict.Add(EPlayerMovementState.Walk, new PlayerWalkState(EPlayerMovementState.Walk, this));
            _StatesDict.Add(EPlayerMovementState.Sprint, new PlayerSprintState(EPlayerMovementState.Sprint, this));
            _StatesDict.Add(EPlayerMovementState.Crouch, new PlayerCrouchState(EPlayerMovementState.Crouch, this));
            _StatesDict.Add(EPlayerMovementState.WallRun, new PlayerWallRunState(EPlayerMovementState.WallRun, this));

            _currentState = _StatesDict[EPlayerMovementState.Walk];

            //Initialize Variables
            CurrentStamina = MaxStamina;
        }

        private void OnEnable()
        {
            _playerInputHandler.OnJumpInput += OnJumpPressed;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnJumpInput -= OnJumpPressed;
        }

        protected override void Update()
        {
            base.Update();

            _movementModule.SetDrag(IsGrounded ? _groundDrag : 0f);

            if (_jumpBuffer && IsGrounded)
            {
                Jump();
            }

            LimitMaxSpeed();

            RegenerateStamina();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            HandleMovement();
        }

        private void HandleMovement()
        {
            if (_movementForce == 0f)
                return;

            _moveDirection = _cameraRootTransform.right * _playerInputHandler.MoveInput.x 
                + _cameraRootTransform.transform.forward * _playerInputHandler.MoveInput.y;

            if (IsOnSlope)
            {
                _moveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeChecker.HitInfo.normal);
                
                if (_playerInputHandler.MoveInput == Vector2.zero && IsGrounded)
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
                if (_currentState.StateKey != EPlayerMovementState.WallRun)
                {
                    _movementModule.SetUseGravity(true);
                }
            }

            _movementModule.AddForce(_moveDirection * _movementForce * (IsGrounded ? 1 : _airMovementMultiplier), ForceMode.Force);
        }

        private void LimitMaxSpeed()
        {
            if (!IsOnSlope)
            {
                _horizontalVelocity.Set(_movementModule.CurrentVelocity.x, 0f, _movementModule.CurrentVelocity.z);

                if (_horizontalVelocity.magnitude > _maxSpeed)
                {
                    _horizontalVelocity = _horizontalVelocity.normalized * _maxSpeed;
                    _movementModule.SetVelocity(_horizontalVelocity.x, _movementModule.CurrentVelocity.y, _horizontalVelocity.z);
                }
            }
            else
            {
                if (_movementModule.CurrentVelocity.magnitude > _maxSpeed)
                {
                    _movementModule.SetVelocity(_movementModule.CurrentVelocity.normalized * _maxSpeed);
                }
            }
            
        }

        private void OnJumpPressed()
        {
            //Set jump buffer that can be consumed upon request to perform certain actions, etc. jump and wall jump
            _jumpBuffer = true;

            if (_jumpBufferCO != null)
            {
                StopCoroutine(_jumpBufferCO);
            }

            _jumpBufferCO = StartCoroutine(ResetJumpBufferCO());
        }

        public void ConsumeJumpBuffer()
        {
            _jumpBuffer = false;

            if (_jumpBufferCO != null)
            {
                StopCoroutine(_jumpBufferCO);
            }
        }

        private IEnumerator ResetJumpBufferCO()
        {
            yield return WaitHandler.GetWaitForSeconds(_jumpBufferDuration);

            ConsumeJumpBuffer();
        }

        private void Jump()
        {
            ConsumeJumpBuffer();

            //Reset Y velocity
            _movementModule.SetYVelocity(0f);

            //Add jump force
            _movementModule.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        public void SetMovementForce(float movementForce)
        {
            this._movementForce = movementForce;
        }

        public void RegenerateStamina()
        {
            if (CurrentStamina >= MaxStamina)
                return;

            if (_staminaRegenTimer <= 0f)
            {
                _staminaRegenTimer = 0f;
                SetStamina(CurrentStamina + _staminaRegenRate * Time.deltaTime);
            }
            else
            {
                _staminaRegenTimer -= Time.deltaTime;
            }
        }

        public void SetStamina(float value)
        {
            CurrentStamina = Mathf.Clamp(value, 0f, MaxStamina);

            OnStaminaChanged?.Invoke(CurrentStamina / MaxStamina);
        }

        public void StartStaminaRegenTimer()
        {
            _staminaRegenTimer = _staminaRegenDelay;
        }

        public void WallRunStarted(bool tiltRight)
        {
            OnWallRunStarted?.Invoke(tiltRight);
        }

        public void WallRunEnded()
        {
            OnWallRunEnded?.Invoke();
        }
    }
}