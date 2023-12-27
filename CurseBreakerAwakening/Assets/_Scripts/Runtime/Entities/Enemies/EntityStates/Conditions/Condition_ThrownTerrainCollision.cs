using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Condition_ThrownTerrainCollision : Condition
    {
        private GrabbableObject _grabbableObject;
        private bool _collidedWithTerrain;

        public Condition_ThrownTerrainCollision(GrabbableObject grabbableObject)
        {
            this._grabbableObject = grabbableObject;
        }

        public override void Enter()
        {
            base.Enter();

            _grabbableObject.OnThrowCollision.AddListener(OnTerrainCollision);
            _collidedWithTerrain = false;
        }

        public override void Exit()
        {
            base.Exit();

            _grabbableObject.OnThrowCollision.RemoveListener(OnTerrainCollision);
            _collidedWithTerrain = false;
        }

        private void OnTerrainCollision()
        {
            _collidedWithTerrain = true;
        }

        public override bool Evaluate()
        {
            return _collidedWithTerrain;
        }

    }
}