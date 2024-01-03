using CBA.Modules;

namespace CBA.Entities
{
    public class IdleState : EnemyState
    {
        private readonly AINavigationModule _navigationModule;

        public IdleState(Entity entity, EnemyStateMachine context) : base(entity, context)
        {
            _navigationModule = _context.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            _navigationModule.SetSpeed(0f);
            _navigationModule.StopFollow();
            _navigationModule.SetLookTarget(null);
        }
    }
}