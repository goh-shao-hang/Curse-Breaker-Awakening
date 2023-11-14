using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerCrouchState : PlayerMovementBaseState
    {
        public PlayerCrouchState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _playerController.PlayerCollider.height = 1f;
            _playerController.SetMovementForce(_playerController.CrouchMovementForce);
            _playerController.MovementModule.AddForce(Vector3.down * 5f, ForceMode.Impulse); //Immediately push player to ground

            _playerController.GroundChecker.SetOffset(_playerController.ColliderBottomPoint); //Set the bottom point for ground check to the new bottom point of the collider
        }

        public override void Exit()
        {
            base.Exit();

            _playerController.PlayerCollider.height = 2f;

            _playerController.GroundChecker.SetOffset(_playerController.ColliderBottomPoint); //Set the bottom point for ground check to the new bottom point of the collider
        }

        public override EPlayerMovementState GetNextState()
        {
            if (!_playerController.PlayerInputHandler.CrouchInput)
            {
                if (!_playerController.PlayerInputHandler.SprintInput || _playerController.PlayerInputHandler.MoveInput == Vector2.zero
                || _playerController.CurrentStamina <= 0f)
                {
                    return EPlayerMovementState.Sprint;
                }
                else
                {
                    return EPlayerMovementState.Walk;
                }
            }

            return this.StateKey;
        }
    }
}