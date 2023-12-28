using CBA.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingAnimationBehaviour : StateMachineBehaviour
{
    [Tooltip("This should be same as the transition time of any animation into the blocking animation.")]
    [SerializeField] private float _blockActivationDelay = 0.2f;

    private PlayerHurtbox _playerHurtbox = null;
    private PlayerCombatManager _playerCombatManager = null;
    
    private float _timeEntered = 0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerHurtbox == null)
        {
            _playerHurtbox = animator.GetComponentInParent<PlayerHurtbox>();
            _playerCombatManager = animator.GetComponentInParent<PlayerCombatManager>();
        }

        _timeEntered = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerHurtbox.IsBlocking) //Already entered blocking state, no need to evaluate anymore
            return;

        _timeEntered += Time.deltaTime;
        if (_timeEntered >= _blockActivationDelay)
        {
            _playerHurtbox.SetIsParrying(true);
        }

        if (_timeEntered >= _blockActivationDelay + _playerCombatManager.CurrentWeapon.WeaponData.ParryDuration)
        {
            _playerHurtbox.SetIsParrying(false);
            _playerHurtbox.SetIsBlocking(true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool isBlocking = animator.GetCurrentAnimatorStateInfo(0).IsName("Block Success");
        bool isParrying = animator.GetCurrentAnimatorStateInfo(0).IsName("Parry");

        if (isBlocking || isParrying)
            return;

        _playerHurtbox.SetIsParrying(false);
        _playerHurtbox.SetIsBlocking(false);
    }

}
