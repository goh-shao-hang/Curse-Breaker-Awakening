using System.Collections;
using System.Collections.Generic;
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
            if (_playerController.PlayerInputHandler.SprintInput)
            {
                return EPlayerMovementState.Sprint;
            }

            return this.StateKey;
        }
    }
}