using CBA.Entities;
using GameCells.StateMachine;

public class Condition_GuardRecovered : Condition
{
    private readonly GuardModule _guardModule;

    public override void Enter()
    {
        base.Enter();
    }

    public Condition_GuardRecovered(GuardModule guardModule)
    {
        this._guardModule = guardModule;
    }

    public override bool Evaluate()
    {
        return !_guardModule.IsGuardBroken;
    }
}
