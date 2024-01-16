using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    [RequireComponent(typeof(Collider))]
    public class EnemyHurtbox : MonoBehaviour, IDamageable
    {
        private Collider _hurtboxCollider;

        [Header("Health")]
        [SerializeField] private HealthModule _healthModule;

        [Header("Guard (Optional)")]
        [SerializeField] private GuardModule _guardModule;

        [Header("Extras")]
        [SerializeField] private Animator _animator;

        [field: SerializeField] public bool IsGuarding { get; private set; } = false;

        public event Action<float> OnHurt;
        public event Action OnParried;

        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private SkinnedMeshRenderer skinnedMeshRenderer => _skinnedMeshRenderer ??= GetComponentInChildren<SkinnedMeshRenderer>();

        private void Awake()
        {
            _hurtboxCollider = GetComponent<Collider>();
        }

        public void SetIsGuarding(bool isGuarding)
        {
            IsGuarding = isGuarding;
        }

        public void TakeDamage(DamageData damageData)
        {
            OnHurt?.Invoke(damageData.DamageAmount);

            if (_guardModule != null && !_guardModule.IsGuardBroken)
            {
                _guardModule.TakeDamage(damageData.DamageAmount);

                if (damageData.IsGuardDamage)
                {
                    OnParried?.Invoke();
                }
            }

            if (!IsGuarding && !damageData.IsGuardDamage)
            {
                _healthModule.TakeDamage(damageData.DamageAmount);
            }

            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.material.DOKill(true);

                skinnedMeshRenderer.material.SetFloat(GameData.DAMAGE_STRENGTH, 1);
                skinnedMeshRenderer.material.DOFloat(0, GameData.DAMAGE_STRENGTH, GameData.DAMAGE_EFFECT_DURATION);
            }

            if (_animator != null)
                _animator.SetTrigger(GameData.HIT_HASH);
        }

        public void SetDamagedAnimationWeight(float weight)
        {
            if (_animator != null)
                _animator.SetLayerWeight((int)GameData.EEnemyAnimatorLayers.Damage, weight);
        }

        public void Enable()
        {
            _hurtboxCollider.enabled = true;
        }

        public void Disable()
        {
            _hurtboxCollider.enabled = false;
        }
    }
}