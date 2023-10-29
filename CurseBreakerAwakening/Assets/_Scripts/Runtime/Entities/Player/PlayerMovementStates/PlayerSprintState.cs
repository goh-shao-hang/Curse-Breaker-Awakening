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

        public override EPlayerMovementState GetNextState()
        {
            if (!_playerController.PlayerInputHandler.SprintInput) //Sprint Released
            {
                return EPlayerMovementState.Walk;
            }

            return this.StateKey;
        }
    }
}