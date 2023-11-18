using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerChargingState : PlayerMovementBaseState
    {
        public PlayerChargingState(EPlayerMovementState key, PlayerController playerController) : base(key, playerController)
        {
        }

        public override void Enter()
        {
            base.Enter();

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override EPlayerMovementState GetNextState()
        {
            }

            return this.StateKey;
        }
    }
}