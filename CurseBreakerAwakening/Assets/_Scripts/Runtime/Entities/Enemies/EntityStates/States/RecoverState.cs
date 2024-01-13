using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class RecoverState : EnemyState
    {
        private readonly GrabbableObject _grabbableObject;
        private readonly AINavigationModule _navigationModule;

        public RecoverState(Entity entity, EnemyStateMachine context, GrabbableObject grabbableObject) : base(entity, context)
        {
            _navigationModule = _entity.GetModule<AINavigationModule>();
            _grabbableObject = grabbableObject;
        }

        public override void Enter()
        {
            base.Enter();

            _context.transform.rotation = Quaternion.Euler(0f, _context.transform.rotation.eulerAngles.y, 0f);
            _context.Animator.CrossFade(GameData.RECOVER_ANIM, 0f, 0);
        }

        public override void Exit()
        {
            base.Exit();

            _navigationModule?.Enable();
            _grabbableObject.EnableThrowPhysics(false);
        }
    }
}