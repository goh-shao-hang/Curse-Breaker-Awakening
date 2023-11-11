using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] protected SO_EntityData _entityData;

        protected float _currentHealth;

        protected virtual void Awake()
        {
            _currentHealth = _entityData.Health;
        }

        public virtual void TakeDamage(float amount)
        {
            _currentHealth -= amount;

            if (_currentHealth <= 0f)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
        }

    }
}