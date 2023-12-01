using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class StunnedState : EntityState
    {
        private GrabbableObject _grabbableObject;

        public StunnedState(Entity entity, GrabbableObject grabbableObject) : base(entity)
        {
            this._grabbableObject = grabbableObject;
        }

        public override void Enter()
        {
            base.Enter();

            _entity.Animator.SetBool(GameData.ISSTUNNED_HASH, true);

            _entity.NavMeshAgentModule?.Disable();

            //TODO set this when exiting some sort of guard state instead
            _entity.Hurtbox.SetIsGuarding(false);

            _grabbableObject?.SetIsGrabbable(true);
        }

        public override void Exit()
        {
            base.Exit();

            _entity.Animator.SetBool(GameData.ISSTUNNED_HASH, false);

            _entity.NavMeshAgentModule?.Enable();

            _grabbableObject?.SetIsGrabbable(false);

            _entity.GuardModule?.ReplenishGuard();
        }
    }
}