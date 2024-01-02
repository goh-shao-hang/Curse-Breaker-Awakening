using CBA;
using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class SpellcastState : EnemyState
    {
        private readonly Spell _spell;
        private readonly int _animationHash;
        private readonly bool _canMoveWhenCasting;
        private AINavigationModule _navigationModule;

        public SpellcastState(Entity entity, EnemyStateMachine context, Spell shield, int animationHash, bool canMoveWhenCasting = false) : base(entity, context)
        {
            _spell = shield;
            _animationHash = animationHash;
            _canMoveWhenCasting = canMoveWhenCasting;

            _navigationModule = _context.GetModule<AINavigationModule>();
        }

        public override void Enter()
        {
            base.Enter();

            if (!_canMoveWhenCasting)
            {
                _navigationModule.StopFollow();
                _navigationModule.SetSpeed(0f);
            }

            _context.Animator.SetTrigger(_animationHash);
            _spell.Activate();
        }
    }
}