using CBA.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class EngagedState : EnemyState
    {
        private readonly float _targetDistance;
        private readonly float _moveSpeed;
        private readonly AINavigationModule _navigationModule;

        private const float _bias = 1f;

        public EngagedState(Entity entity, EnemyStateMachine context, float targetDistance, float moveSpeed) : base(entity, context)
        {
            this._targetDistance = targetDistance;
            this._moveSpeed = moveSpeed;
            this._navigationModule = _context.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule.SetLookTarget(_entity.PlayerPos);

            float distance = Vector3.Distance(_context.transform.position, _entity.PlayerPos.Value);
            if (distance > _targetDistance + _bias)
            {
                _navigationModule.SetSpeed(_moveSpeed);

                _navigationModule.SetFollowPosition(_entity.PlayerPos);
            }
            else if (distance < _targetDistance - _bias)
            {
                _navigationModule.SetSpeed(_moveSpeed);

                Vector3 retreatDirection = -(_entity.PlayerPos.Value - _entity.transform.position).normalized;
                retreatDirection.y = 0f;

                _navigationModule.SetOneTimeDestination(_entity.transform.position + retreatDirection);
            }
            else
            {
                _navigationModule.SetSpeed(0f);
            }
        }

        public override void Update()
        {
            base.Update();

            float distance = Vector3.Distance(_context.transform.position, _entity.PlayerPos.Value);
            if (distance > _targetDistance + _bias)
            {
                _navigationModule.SetSpeed(_moveSpeed);

                _navigationModule.SetFollowPosition(_entity.PlayerPos);
            }
            else if (distance < _targetDistance - _bias)
            {
                _navigationModule.SetSpeed(_moveSpeed);

                Vector3 retreatDirection = -(_entity.PlayerPos.Value - _entity.transform.position).normalized;
                retreatDirection.y = 0f;

                _navigationModule.SetOneTimeDestination(_entity.transform.position + retreatDirection);
            }
            else
            {
                _navigationModule.SetFollowPosition(_entity.PlayerPos);
                _navigationModule.SetSpeed(0f);
            }
        }
    }
}