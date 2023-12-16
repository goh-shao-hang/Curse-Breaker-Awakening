using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerSprintState : PlayerMovementBaseState
    {
        public PlayerSprintState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _playerController.SetMovementForce(_playerController.SprintMovementForce);
        }

        public override void Update()
        {
            base.Update();

            ConsumeStamina();
        }

        private void ConsumeStamina()
        {
            _playerController.SetStamina(_playerController.CurrentStamina - _playerController.SpritingStaminaConsumption * Time.deltaTime);
            _playerController.StartStaminaRegenTimer();
        }

        public override EPlayerMovementState GetNextState()
        {
            if (_playerController.IsPerformingChargedAttack)
            {
                return EPlayerMovementState.ChargedAttack;
            }

            if (_playerController.CanWallRun && _playerController.IsRunnableWallDetected) //&& Mathf.Abs(_playerController.PlayerInputHandler.MoveInput.y) >= 0.1f)
            {
                return EPlayerMovementState.WallRun;
            }

            if (_playerController.IsGrounded && _playerController.PlayerInputHandler.CrouchInput)
            {
                return EPlayerMovementState.Crouch;
            }

            if (!_playerController.PlayerInputHandler.SprintInput || _playerController.PlayerInputHandler.MoveInput == Vector2.zero 
                || _playerController.CurrentStamina <= 0f)
            {
                return EPlayerMovementState.Walk;
            }

            return this.StateKey;
        }
    }
}