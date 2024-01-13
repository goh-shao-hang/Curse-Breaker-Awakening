using CBA.Modules;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class DeathState : EnemyState
    {
        //TODO make ragdoll controller subsribe to entity on death instead
        private readonly RagdollController _ragdollController = null; //Optional
        private readonly GrabbableObject _grabbableObject = null;

        private readonly AINavigationModule _navigationModule;

        public DeathState(Entity entity, EnemyStateMachine context, RagdollController ragdollController = null, GrabbableObject grabbableObject = null) : base(entity, context)
        {
            this._navigationModule = _entity.GetModule<AINavigationModule>();

            this._ragdollController = ragdollController;
            this._grabbableObject = grabbableObject;
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule?.Disable();
            _grabbableObject?.SetIsGrabbable(false);
            _context.Hurtbox.Disable();
            _ragdollController?.EnableRagdoll();

            _context.Animator.SetTrigger(GameData.DIE_HASH);

            _entity.Die();
        }
    }
}