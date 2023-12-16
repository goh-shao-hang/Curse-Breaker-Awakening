using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    // unused
    public class PlayerChargingState : PlayerMovementBaseState
    {
        public PlayerChargingState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Charging start");
            _playerController.SetMovementForce(_playerController.ChargingMoveForce);
        }

        public override void Exit()
        {
            base.Exit();

        }

        public override EPlayerMovementState GetNextState()
        {
            if (!_playerController.IsChargingAttack)
            {
                return EPlayerMovementState.ChargedAttack;
            }

            return this.StateKey;
        }
    }
}