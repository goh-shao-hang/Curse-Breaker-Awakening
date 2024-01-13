using CBA.Modules;

namespace CBA.Entities
{
    public class ChaseState : EnemyState
    {
        private readonly AINavigationModule _navigationModule;
        private readonly float _chaseSpeed;

        public ChaseState(Entity entity, EnemyStateMachine context, float chaseSpeed = 1f) : base(entity, context)
        {
            _navigationModule = _entity.GetModule<AINavigationModule>();
            _chaseSpeed = chaseSpeed;
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule.SetFollowPosition(_entity.PlayerPos);
            _navigationModule.SetLookTarget(_entity.PlayerPos);
            _navigationModule.SetSpeed(_chaseSpeed);
        }

    }
}