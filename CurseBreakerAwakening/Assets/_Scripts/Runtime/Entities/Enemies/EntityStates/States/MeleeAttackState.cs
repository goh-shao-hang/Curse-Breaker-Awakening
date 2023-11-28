using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class MeleeAttackState : EntityState
    {
        public MeleeAttackState(Entity entity) : base(entity)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _entity.NavMeshAgentModule.StopFollow();
            _entity.Animator.SetTrigger(GameData.ATTACK_HASH);
        }
    }
}