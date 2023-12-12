using CBA.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingAnimationBehaviour : StateMachineBehaviour
{
    [Tooltip("This should be same as the transition time of any animation into the blocking animation.")]
    [SerializeField] private float _blockActivationDelay = 0.2f;

    private PlayerCombatManager _playerCombatManager = null;
    private float _timeEntered = 0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerCombatManager == null)
        {
            _playerCombatManager = animator.GetComponentInParent<PlayerCombatManager>();
        }

        _timeEntered = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerCombatManager.IsBlocking)
            return;

        _timeEntered += Time.deltaTime;
        if (_timeEntered >= _blockActivationDelay)
        {
            _playerCombatManager.SetIsBlocking(true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombatManager.SetIsBlocking(false);
    }

}
