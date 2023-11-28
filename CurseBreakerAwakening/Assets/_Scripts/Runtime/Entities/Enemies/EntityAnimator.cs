using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class EntityAnimator : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _healthModule;
        [SerializeField] private GuardModule _guardModule;
        [SerializeField] private Animator _animator;

        public Animator Animator => _animator;

        private void OnEnable()
        {
            _healthModule.OnHealthChanged.AddListener(PlayHitAnimation);

            //TODO
            _guardModule.OnGuardBroken.AddListener(PlayStunnedAnimation);
        }

        private void OnDisable()
        {
            _healthModule.OnHealthChanged.RemoveListener(PlayHitAnimation);

            //TODO
            _guardModule.OnGuardBroken.RemoveListener(PlayStunnedAnimation);

        }

        private void PlayHitAnimation()
        {
            _animator.SetTrigger(GameData.HIT_HASH);
        }

        private void PlayStunnedAnimation()
        {
            _animator.SetTrigger(GameData.ISSTUNNED_HASH);
        }

        private void PlayGrabbedAnimation()
        {
            _animator.SetTrigger(GameData.ISGRABBED_HASH);
        }
    }
}