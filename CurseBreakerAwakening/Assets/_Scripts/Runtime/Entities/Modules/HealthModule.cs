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

        public UnityEvent OnHealthChanged;
        public UnityEvent OnHealthDepleted;

        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }

        private void Start()
        {
            MaxHealth = _entity.EntityData.Health;
            CurrentHealth = _entity.EntityData.Health;
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            OnHealthChanged?.Invoke();

            if (CurrentHealth <= 0f)
            {
                OnHealthDepleted?.Invoke();
            }
        }
    }
}