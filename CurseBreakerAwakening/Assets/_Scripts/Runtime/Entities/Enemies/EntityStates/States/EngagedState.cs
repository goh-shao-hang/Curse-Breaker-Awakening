using CBA.Modules;
using UnityEngine;

namespace CBA.Entities
{
    public class EngagedState : EnemyState
    {
        private readonly float _engageDistance;
        private readonly float _engageDistanceBias = 1f;
        private readonly float _moveSpeed;
        private readonly float _strafeSpeed;
        private readonly AINavigationModule _navigationModule;

        private int _strafeDirection = 0;

        public EngagedState(Entity entity, EnemyStateMachine context, float engageDistance, float engageDistanceBias, float moveSpeed, float strafeSpeed) : base(entity, context)
        {
            this._engageDistance = engageDistance;
            this._engageDistanceBias = engageDistanceBias;
            this._moveSpeed = moveSpeed;
            this._strafeSpeed = strafeSpeed;
            this._navigationModule = _entity.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule.SetLookTarget(_entity.PlayerPos);

            EngagedMovement();
        }

        public override void Update()
        {
            base.Update();

            EngagedMovement();
        }

        private void EngagedMovement()
        {
            float distance = Vector3.Distance(_context.transform.position, _entity.PlayerPos.Value);
            if (distance > _engageDistance + _engageDistanceBias)
            {
                _strafeDirection = 0;

                _navigationModule.SetSpeed(_moveSpeed);

                _navigationModule.SetFollowPosition(_entity.PlayerPos);
            }
            else if (distance < _engageDistance - _engageDistanceBias)
            {
                _strafeDirection = 0;

                _navigationModule.SetSpeed(_moveSpeed);

                Vector3 retreatDestination = -(_entity.PlayerPos.Value - _entity.transform.position).normalized;
                retreatDestination.y = 0f;

                _navigationModule.SetOneTimeDestination(_entity.transform.position + retreatDestination);
            }
            else
            {
                _navigationModule.SetSpeed(_strafeSpeed);

                if (_strafeDirection == 0)
                {
                    _strafeDirection = Random.Range(-1, 2);
                }

                Vector3 direction = _entity.PlayerPos.Value - _entity.transform.position;
                direction.y = 0;
                Vector3 _strafeDestination = Vector3.Cross(direction, Vector3.up).normalized * _strafeDirection;

                _navigationModule.SetOneTimeDestination(_entity.transform.position + _strafeDestination);
            }
        }
    }
}