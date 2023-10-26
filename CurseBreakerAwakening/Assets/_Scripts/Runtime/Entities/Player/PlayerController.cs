using GameCells.Modules;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private MovementModule _movementModule;
        [SerializeField] private PhysicsQuery _groundChecker;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _walkMovementForce = 50f;
        [SerializeField] private float _sprintMovementForce = 100f;
        [SerializeField] private float _maxSpeed = 8f;
        [SerializeField] private float _groundDrag = 7f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _airMovementMultiplier = 0.1f;

        public bool IsGrounded => _groundChecker.Hit();

        private float _movementForce;
        private Vector3 _targetVelocity;

        private void Update()
        {
            _movementModule.SetDrag(IsGrounded ? _groundDrag : 0f);

            if (_playerInputHandler.JumpInput && IsGrounded)
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            _movementForce = _walkMovementForce;

            _targetVelocity.Set(_playerInputHandler.MoveInput.x, 0f, _playerInputHandler.MoveInput.y);
            _targetVelocity = transform.TransformDirection(_targetVelocity * _movementForce * (IsGrounded ? 1 : _airMovementMultiplier));

            _movementModule.AddForce(_targetVelocity, ForceMode.Force);
        }

        private void Jump()
        {
            //Reset Y velocity
            _movementModule.SetYVelocity(0f);

            //Add jump force
            _movementModule.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }
}