using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Entities
{
    public class GuardModule : Module
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Entity _entity;

        public UnityEvent OnGuardMeterChanged;
        public UnityEvent OnGuardBroken;

        public float CurrentGuardMeter { get; private set; }
        public float MaxGuardMeter { get; private set; }

        private void Start()
        {
            CurrentGuardMeter = _entity.EntityData.Guard;
        }

        public void ReplenishGuard()
        {
            CurrentGuardMeter = MaxGuardMeter;
        }

        public void SetGuard(float guard)
        {
            CurrentGuardMeter = guard;
        }

        public void TakeDamage(float amount)
        {
            CurrentGuardMeter -= amount;
            OnGuardMeterChanged?.Invoke();

            if (CurrentGuardMeter <= 0f)
            {
                OnGuardBroken?.Invoke();
            }
        }
    }
}