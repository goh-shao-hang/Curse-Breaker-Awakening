using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class StunnedState : EnemyState
    {
        private GrabbableObject _grabbableObject;

        private readonly AINavigationModule _navigationModule;
        private readonly GuardModule _guardModule;

        public StunnedState(Entity entity, EnemyStateMachine context, GrabbableObject grabbableObject) : base(entity, context)
        {
            this._grabbableObject = grabbableObject;

            this._navigationModule = _context.ModuleManager.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            _context.Hurtbox.SetDamagedAnimationWeight(1f);
            _context.Animator.SetBool(GameData.ISSTUNNED_HASH, true);

            _navigationModule?.SetFollowPosition(null);
            _navigationModule?.SetLookTarget(null);
            _navigationModule?.SetSpeed(0);

            //TODO set this when exiting some sort of guard state instead
            _context.Hurtbox.SetIsGuarding(false);

            _grabbableObject?.SetIsGrabbable(true);
        }

        public override void Exit()
        {
            base.Exit();

            _context.Hurtbox.SetDamagedAnimationWeight(0.3f);
            _context.Animator.SetBool(GameData.ISSTUNNED_HASH, false);

            _grabbableObject?.SetIsGrabbable(false);

            _guardModule?.ReplenishGuard();
        }
    }
}