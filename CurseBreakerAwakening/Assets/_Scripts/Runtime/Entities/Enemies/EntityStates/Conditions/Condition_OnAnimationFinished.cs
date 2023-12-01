using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Condition_OnAnimationFinished : Condition
    {
        private Animator _animator;

        public Condition_OnAnimationFinished(Animator animator)
        {
            _animator = animator;
        }



        public override bool Evaluate()
        {
            Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }

    }
}