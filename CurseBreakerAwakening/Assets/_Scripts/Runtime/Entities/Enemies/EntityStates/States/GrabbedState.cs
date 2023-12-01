using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class GrabbedState : EnemyState
    {
        private GrabbableObject _grabbable;
        private bool _isGrabbed;

        public GrabbedState(Entity entity, EnemyStateMachine context, GrabbableObject grabbable) : base(entity, context)
        {
            this._grabbable = grabbable;
        }

        public override void Enter()
        {
            base.Enter();

            SetGrabbableState(true);

            _grabbable.OnThrown.AddListener(() => SetGrabbableState(false)); //No longer grabbable upon being thrown
            _grabbable.OnTerrainCollision.AddListener(() => EnableThrowPhysics(false)); //No longer use physics upon hitting terrain

            _context.Animator.SetBool(GameData.ISGRABBED_HASH, true);

            _context.NavMeshAgentModule?.Disable();

            _context.GuardModule?.SetGuard(0f);
        }

        public override void Exit()
        {
            base.Exit();

            SetGrabbableState(false);

            _grabbable.OnThrown.RemoveListener(() => SetGrabbableState(false));
            _grabbable.OnTerrainCollision.RemoveListener(() => EnableThrowPhysics(false));

            _context.Animator.SetBool(GameData.ISGRABBED_HASH, false);

            _context.NavMeshAgentModule?.Enable();

            _context.GuardModule?.ReplenishGuard();
        }

        private void SetGrabbableState(bool isGrabbable)
        {
            _grabbable.SetIsGrabbable(isGrabbable);
        }

        private void EnableThrowPhysics(bool enabled)
        {
            _grabbable.EnableThrowPhysics(enabled);
        }
    }
}