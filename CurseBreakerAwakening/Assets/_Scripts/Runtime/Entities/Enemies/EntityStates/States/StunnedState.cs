using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class StunnedState : EnemyState
    {
        private GrabbableObject _grabbableObject;

        public StunnedState(Entity entity, EnemyStateMachine context, GrabbableObject grabbableObject) : base(entity, context)
        {
            this._grabbableObject = grabbableObject;
        }

        public override void Enter()
        {
            base.Enter();

            _context.Hurtbox.SetDamagedAnimationWeight(1f);
            _context.Animator.SetBool(GameData.ISSTUNNED_HASH, true);

            _context.NavMeshAgentModule?.Disable();

            //TODO set this when exiting some sort of guard state instead
            _context.Hurtbox.SetIsGuarding(false);

            _grabbableObject?.SetIsGrabbable(true);
        }

        public override void Exit()
        {
            base.Exit();

            _context.Hurtbox.SetDamagedAnimationWeight(0.3f);
            _context.Animator.SetBool(GameData.ISSTUNNED_HASH, false);

            _context.NavMeshAgentModule?.Enable();

            _grabbableObject?.SetIsGrabbable(false);

            _context.GuardModule?.ReplenishGuard();
        }
    }
}