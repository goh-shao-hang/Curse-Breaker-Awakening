using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class GrabbedState : EnemyState
    {
        private GrabbableObject _grabbable;
        private bool _isGrabbed;

        private readonly AINavigationModule _navigationModule;
        private readonly GuardModule _guardModule;
        private readonly GrabbableObject _grabbableObject;

        public GrabbedState(Entity entity, EnemyStateMachine context, GrabbableObject grabbable) : base(entity, context)
        {
            this._grabbable = grabbable;

            this._guardModule = _context.ModuleManager.GetModule<GuardModule>();
            this._navigationModule = _context.ModuleManager.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            _grabbable.SetIsGrabbable(false);
            //SetGrabbableState(false);

            //_grabbable.OnThrown.AddListener(() => SetGrabbableState(false)); //No longer grabbable upon being thrown
            //_grabbable.OnThrowCollision.AddListener(() => EnableThrowPhysics(false)); //No longer use physics upon hitting terrain

            _context.Animator.SetBool(GameData.ISGRABBED_HASH, true);

            _navigationModule?.Disable();

            //TODO mind this, and the one below
            _guardModule?.SetGuard(0f);
        }

        public override void Exit()
        {
            base.Exit();

            //SetGrabbableState(false);

            //_grabbable.OnThrown.RemoveListener(() => SetGrabbableState(false));
            //_grabbable.OnThrowCollision.RemoveListener(() => EnableThrowPhysics(false));

            _context.Animator.SetBool(GameData.ISGRABBED_HASH, false);

            _guardModule?.ReplenishGuard();
        }

        /*private void SetGrabbableState(bool isGrabbable)
        {
            _grabbable.SetIsGrabbable(isGrabbable);
        }*/

        private void EnableThrowPhysics(bool enabled)
        {
            _grabbable.EnableThrowPhysics(enabled);
        }
    }
}