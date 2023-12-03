using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedAnimationBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.transform.SetLocalPositionAndRotation(animator.transform.localPosition + Vector3.up, Quaternion.Euler(-90f, 0, 0f));
        LeanTween.moveLocal(animator.gameObject, Vector3.up, 0.05f);
        LeanTween.rotateLocal(animator.gameObject, new Vector3(-90f, 0, 0f), 0.05f);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LeanTween.moveLocal(animator.gameObject, Vector3.zero, 0.2f);
        LeanTween.rotateLocal(animator.gameObject, Vector3.zero, 0.2f);

        //_characterMeshTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
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
