using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class EnemyHurtbox : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        [SerializeField] private HealthModule _healthModule;

        [Header("Guard (Optional)")]
        [SerializeField] private GuardModule _guardModule;

        [field: SerializeField] public bool IsGuarding { get; private set; } = false;

        public void SetIsGuarding(bool isGuarding)
        {
            IsGuarding = isGuarding;
        }

        public void TakeDamage(float amount)
        {
            if (_guardModule != null && IsGuarding)
            {
                _guardModule.TakeDamage(amount);
            }
            else
            {
                _healthModule.TakeDamage(amount);
            }
        }
    }
}