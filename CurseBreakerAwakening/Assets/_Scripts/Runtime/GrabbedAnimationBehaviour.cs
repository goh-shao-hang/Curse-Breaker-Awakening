using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedAnimationBehaviour : StateMachineBehaviour
{
    private Transform _characterMeshTransform = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_characterMeshTransform == null)
        {
            _characterMeshTransform = animator.transform;
        }

        _characterMeshTransform.SetPositionAndRotation(_characterMeshTransform.position + Vector3.up, Quaternion.Euler(-90f, 0, 0f));
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _characterMeshTransform.SetPositionAndRotation(_characterMeshTransform.position - Vector3.up, Quaternion.Euler(0f, 0f, 0f));
    }

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
