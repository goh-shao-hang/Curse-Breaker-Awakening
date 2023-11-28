using GameCells.StateMachine;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Condition_PlayerOutOfRange : Condition
    {
        private PhysicsQuery _playerDetector;

        public Condition_PlayerOutOfRange(PhysicsQuery playerDetector)
        {
            this._playerDetector = playerDetector;
        }

        public override bool Evaluate()
        {
            return !_playerDetector.Hit();
        }
    }
}