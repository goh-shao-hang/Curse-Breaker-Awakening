using CBA;
using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class SpellcastState : EnemyState
    {
        private readonly SpellManager _spellManager;
        private readonly Spell _spell;
        private readonly AINavigationModule _navigationModule;

        public SpellcastState(Entity entity, EnemyStateMachine context, SpellManager spellManager, Spell spell) : base(entity, context)
        {
            _spellManager = spellManager;
            _spell = spell;
            _navigationModule = _context.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log($"ENTERED SPELL {_spell.name}");

            _spellManager.SetCurrentSpell(_spell);

            if (!_spell.CanMoveWhileCasting)
            {
                _navigationModule.StopFollow();
                _navigationModule.SetSpeed(0f);
            }

            _context.Animator.SetTrigger(_spell.SpellAnimationHash);
        }

        public override void Exit()
        {
            base.Exit();

            Debug.Log($"EXITED SPELL {_spell.name}");
        }
    }
}