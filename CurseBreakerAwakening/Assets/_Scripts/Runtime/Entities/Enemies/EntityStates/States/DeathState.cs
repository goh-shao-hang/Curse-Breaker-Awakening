using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class DeathState : EntityState
    {
        private RagdollController _ragdollController = null; //Optional

        public DeathState(Entity entity, RagdollController ragdollController = null) : base(entity)
        {
            this._ragdollController = ragdollController;
        }

        public override void Enter()
        {
            base.Enter();

            _entity.NavMeshAgentModule?.Disable();
            _entity.Hurtbox.Disable();
            _ragdollController?.EnableRagdoll();
        }
    }
}