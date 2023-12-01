using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class RecoverState : EntityState
    {
        public RecoverState(Entity entity) : base(entity)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _entity.Animator.CrossFade("Recover", 0f, 0);
        }
    }
}