using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CBA.Entities
{
    public class GuardModule : MonoBehaviour
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