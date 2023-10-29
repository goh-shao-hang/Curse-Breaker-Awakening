using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public abstract class PlayerMovementBaseState : BaseState<EPlayerMovementState>
    {
        protected PlayerController _playerController;

        protected PlayerMovementBaseState(EPlayerMovementState key, PlayerController playerController) : base(key)
        {
            this._playerController = playerController;
        }
    }
}