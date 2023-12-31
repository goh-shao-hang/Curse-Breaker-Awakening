using CBA.Entities.Player.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAnimationBehaviour : StateMachineBehaviour
{
    private CombatAnimationEventHander _combatAnimationEventHander;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_combatAnimationEventHander == null)
        {
            _combatAnimationEventHander = animator.GetComponent<CombatAnimationEventHander>();
        }

        //Prevents interruption such as being stunned, in those cases the animation wont reach the end thus not tregerring these animation events
        _combatAnimationEventHander.DeactivateHitbox();
        _combatAnimationEventHander.StopEmitTrail();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
