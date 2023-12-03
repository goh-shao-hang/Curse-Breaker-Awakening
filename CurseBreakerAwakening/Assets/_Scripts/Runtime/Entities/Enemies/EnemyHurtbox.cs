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

        private void Awake()
        {
            _hurtboxCollider = GetComponent<Collider>();
        }

        public void SetIsGuarding(bool isGuarding)
        {
            IsGuarding = isGuarding;
        }

        public void TakeDamage(float amount)
        {
            OnHurt?.Invoke(amount);

            if (_guardModule != null && IsGuarding)
            {
                _guardModule.TakeDamage(amount);
            }
            else
            {
                _healthModule.TakeDamage(amount);
                _animator.SetTrigger(GameData.HIT_HASH);
            }
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