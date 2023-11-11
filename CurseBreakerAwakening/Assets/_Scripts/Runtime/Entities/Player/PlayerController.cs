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
        [SerializeField] private PhysicsQuery _groundChecker;
        [SerializeField] private RaycastCheck _slopeChecker;
        [SerializeField] private Animator _animator;

        //Dependency getters
        public PlayerInputHandler PlayerInputHandler => _playerInputHandler;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _walkMovementForce = 50f;
        [SerializeField] private float _sprintMovementForce = 100f;
        [SerializeField] private float _maxSpeed = 15f;
        [SerializeField] private float _groundDrag = 7f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _airMovementMultiplier = 0.1f;
        [SerializeField] private float _maxSlopeAngle = 45f;

        public float WalkMovementForce => _walkMovementForce;
        public float SprintMovementForce => _sprintMovementForce;

        public bool IsGrounded => _groundChecker.Hit();
        public bool IsOnSlope
        {
            get
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeChecker.HitInfo.normal);
                return _slopeChecker.Hit() && slopeAngle != 0 && slopeAngle < _maxSlopeAngle;
            }
        }

        private float _movementForce;
        private Vector3 _moveDirection;
        private Vector3 _horizontalVelocity;

        protected override void Awake()
        {
            base.Awake();

            //State Machine Initialization
            _StatesDict.Add(EPlayerMovementState.Walk, new PlayerWalkState(EPlayerMovementState.Walk, this));
            _StatesDict.Add(EPlayerMovementState.Sprint, new PlayerSprintState(EPlayerMovementState.Sprint, this));

            _currentState = _StatesDict[EPlayerMovementState.Walk];
        }

        protected override void Update()
        {
            base.Update();

            _movementModule.SetDrag(IsGrounded ? _groundDrag : 0f);

            if (_playerInputHandler.JumpInput && IsGrounded)
            {
                Jump();
            }

            LimitMaxSpeed();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            HandleMovement();
        }

        private void HandleMovement()
        {
            _moveDirection = _cameraRootTransform.right * _playerInputHandler.MoveInput.x 
                + _cameraRootTransform.transform.forward * _playerInputHandler.MoveInput.y;

            if (IsOnSlope)
            {
                _moveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeChecker.HitInfo.normal);
                
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

            _movementModule.AddForce(_moveDirection * _movementForce * (IsGrounded ? 1 : _airMovementMultiplier), ForceMode.Force);

            //TODO
            if (_animator == null)
                return;

            _animator.SetBool(GameData.ISMOVING_HASH, _playerInputHandler.MoveInput != Vector2.zero);
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

        private void Jump()
        {
            //Reset Y velocity
            _movementModule.SetYVelocity(0f);

            //Add jump force
            _movementModule.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        public void SetMovementForce(float movementForce)
        {
            this._movementForce = movementForce;
        }
    }
}