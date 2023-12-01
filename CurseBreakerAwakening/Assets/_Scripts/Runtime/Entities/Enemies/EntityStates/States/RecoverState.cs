using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class RecoverState : EnemyState
    {
        public RecoverState(Entity entity, EnemyStateMachine context) : base(entity, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _context.Animator.CrossFade("Recover", 0f, 0);
        }
    }
}