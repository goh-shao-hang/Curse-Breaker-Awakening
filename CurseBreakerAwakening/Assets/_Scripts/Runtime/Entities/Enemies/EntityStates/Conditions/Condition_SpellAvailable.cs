using GameCells.StateMachine;

public class Condition_SpellAvailable : Condition
{
    private readonly Spell _spell;

    public Condition_SpellAvailable(Spell spell)
    {
        this._spell = spell;
    }

    public override bool Evaluate()
    {
        return _spell.IsAvailable;
    }


}
