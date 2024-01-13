using CBA;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Entities
{
    public class GuardModule : Module
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Entity _entity;

        public UnityEvent OnGuardMeterIncreased;
        public UnityEvent OnGuardMeterDecreased;
        public UnityEvent OnGuardBroken;

        public float CurrentGuardMeter { get; private set; }
        public float MaxGuardMeter { get; private set; }

        private float _guardBreakTimer = 0;

        public bool IsGuardBroken { get; private set; }
        public bool CanRecoverFromGuardBreak { get; private set; } = true;

        private void Start()
        {
            MaxGuardMeter = _entity.EntityData.Guard;
            CurrentGuardMeter = MaxGuardMeter;
        }

        private void Update()
        {
            if (!IsGuardBroken) 
                return;

            if (_guardBreakTimer > 0)
            {
                _guardBreakTimer -= Time.deltaTime;
            }
            else if (_guardBreakTimer <= 0 && CanRecoverFromGuardBreak)
            {
                ReplenishGuard();
            }
            
        }

        public void ReplenishGuard()
        {
            _guardBreakTimer = 0f;
            IsGuardBroken = false;

            CurrentGuardMeter = MaxGuardMeter;

            OnGuardMeterIncreased?.Invoke();
        }

        public void SetCanRecoverFromGuardBreak(bool canRecover)
        {
            CanRecoverFromGuardBreak = canRecover;
        }

        public void TakeDamage(float amount)
        {
            CurrentGuardMeter -= amount;
            OnGuardMeterDecreased?.Invoke();

            if (CurrentGuardMeter <= 0f)
            {
                IsGuardBroken = true;
                _guardBreakTimer = _entity.EntityData.BaseStunDuration;

                Debug.LogWarning("BREAK");
                OnGuardBroken?.Invoke();
            }
        }
    }
}