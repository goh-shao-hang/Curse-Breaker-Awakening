using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class DeathState : EnemyState
    {
        private RagdollController _ragdollController = null; //Optional

        public DeathState(Entity entity, EnemyStateMachine context, RagdollController ragdollController) : base(entity, context)
        {
            this._ragdollController = ragdollController;
        }

        public override void Enter()
        {
            base.Enter();

            _context.NavMeshAgentModule?.Disable();
            _context.Hurtbox.Disable();
            _ragdollController?.EnableRagdoll();
        }
    }
}