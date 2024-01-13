using CBA.Modules;
using UnityEngine;

namespace CBA.Entities
{
    public class EngagedState : EnemyState
    {
        private readonly float _engageDistance;
        private readonly float _engageDistanceBias = 1f;
        private readonly float _moveSpeed;
        private readonly AINavigationModule _navigationModule;


        public EngagedState(Entity entity, EnemyStateMachine context, float engageDistance, float engageDistanceBias, float moveSpeed) : base(entity, context)
        {
            this._engageDistance = engageDistance;
            this._engageDistanceBias = engageDistanceBias;
            this._moveSpeed = moveSpeed;
            this._navigationModule = _entity.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule.SetLookTarget(_entity.PlayerPos);

            float distance = Vector3.Distance(_context.transform.position, _entity.PlayerPos.Value);
            if (distance > _engageDistance + _engageDistanceBias)
            {
                _navigationModule.SetSpeed(_moveSpeed);

                _navigationModule.SetFollowPosition(_entity.PlayerPos);
            }
            else if (distance < _engageDistance - _engageDistanceBias)
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
            if (distance > _engageDistance + _engageDistanceBias)
            {
                _navigationModule.SetSpeed(_moveSpeed);

                _navigationModule.SetFollowPosition(_entity.PlayerPos);
            }
            else if (distance < _engageDistance - _engageDistanceBias)
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
    }
}