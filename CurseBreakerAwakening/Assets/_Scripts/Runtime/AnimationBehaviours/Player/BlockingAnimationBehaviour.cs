using CBA.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingAnimationBehaviour : StateMachineBehaviour
{
    private PlayerCombatManager _playerCombatManager = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerCombatManager == null)
        {
            _playerCombatManager = animator.GetComponentInParent<PlayerCombatManager>();
        }

        _playerCombatManager.SetIsBlocking(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombatManager.SetIsBlocking(false);
    }

}
