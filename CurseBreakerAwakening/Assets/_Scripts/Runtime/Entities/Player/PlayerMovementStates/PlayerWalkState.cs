using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerWalkState : PlayerMovementBaseState
    {
        public PlayerWalkState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            _playerController.SetMovementForce(_playerController.WalkMovementForce);
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

            if (_playerController.CurrentStamina > 0 && _playerController.PlayerInputHandler.SprintInput 
                && _playerController.PlayerInputHandler.MoveInput != Vector2.zero)
            {
                return EPlayerMovementState.Sprint;
            }

            return this.StateKey;
        }
    }
}