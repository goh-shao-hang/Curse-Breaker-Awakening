using CBA.Modules;

namespace CBA.Entities
{
    public class ChaseState : EnemyState
    {
        private readonly AINavigationModule _navigationModule;
        private readonly float _chaseSpeed;

        public ChaseState(Entity entity, EnemyStateMachine context, float chaseSpeed = 1f) : base(entity, context)
        {
            _navigationModule = _context.ModuleManager.GetModule<AINavigationModule>();
            _chaseSpeed = chaseSpeed;
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule.SetFollowPosition(_entity._playerPos);
            _navigationModule.SetSpeed(_chaseSpeed);
        }

    }
}