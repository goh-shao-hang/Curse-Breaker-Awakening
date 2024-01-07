using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Entities
{
    public class HealthModule : Module
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Entity _entity;

        public UnityEvent OnHealthIncreased;
        public UnityEvent OnHealthDecreased;
        public UnityEvent OnHealthDepleted;

        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }

        public bool IsInvincible { get; private set; } = false;

        private void Start()
        {
            MaxHealth = _entity.EntityData.Health;
            CurrentHealth = _entity.EntityData.Health;
        }

        public void SetInvincibility(bool invincible)
        {
            IsInvincible = invincible;
        }

        public void RestoreHealth(float amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;

            OnHealthIncreased?.Invoke();
        }

        public void TakeDamage(float amount)
        {
            if (IsInvincible)
                return;

            CurrentHealth -= amount;
            OnHealthDecreased?.Invoke();

            if (CurrentHealth <= 0f)
            {
                CurrentHealth = 0f;
                OnHealthDepleted?.Invoke();
            }
        }
    }
}