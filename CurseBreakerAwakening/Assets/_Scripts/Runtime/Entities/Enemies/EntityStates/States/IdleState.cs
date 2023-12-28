using CBA.Modules;

namespace CBA.Entities
{
    public class IdleState : EnemyState
    {
        public IdleState(Entity entity, EnemyStateMachine context) : base(entity, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _context.ModuleManager.GetModule<AINavigationModule>().SetSpeed(0f);
            _context.ModuleManager.GetModule<AINavigationModule>().StopFollow();
        }
    }
}