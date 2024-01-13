using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class RetreatState : EnemyState
    {
        private readonly AINavigationModule _navigationModule;
        private readonly float _retreatSpeed;

        public RetreatState(Entity entity, EnemyStateMachine context, float retreatSpeed) : base(entity, context)
        {
            _navigationModule = _entity.GetModule<AINavigationModule>();
            _retreatSpeed = retreatSpeed;
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule.SetSpeed(_retreatSpeed);

            Vector3 retreatDirection = -(_entity.PlayerPos.Value - _entity.transform.position).normalized;
            retreatDirection.y = 0f;

            _navigationModule.SetOneTimeDestination(_entity.transform.position + retreatDirection);
        }

        public override void Update()
        {
            base.Update();

            Vector3 retreatDirection = -(_entity.PlayerPos.Value - _entity.transform.position).normalized ;
            retreatDirection.y = 0f;

            _navigationModule.SetOneTimeDestination(_entity.transform.position + retreatDirection);
        }
    }
}