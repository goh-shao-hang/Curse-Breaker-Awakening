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
        private RagdollController _ragdollController = null; //Optional

        private readonly AINavigationModule _navigationModule;

        public DeathState(Entity entity, EnemyStateMachine context, RagdollController ragdollController = null) : base(entity, context)
        {
            if (ragdollController != null)
                this._ragdollController = ragdollController;

            this._navigationModule = _context.ModuleManager.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule?.Disable();
            _context.Hurtbox.Disable();
            _ragdollController?.EnableRagdoll();

            _entity.Die();
        }
    }
}