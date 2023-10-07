using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [SerializeField] private float _initialMaxStamina = 100f;
        [SerializeField] private float _staminaRegenRate = 5f;
        [SerializeField] private float _staminaRegenDelay = 1.5f;

        public float MaxStamina { get; private set; }
        public float CurrentStamina { get; private set; }

        private Coroutine _staminaRegenDelayCO;

        public event Action OnMaxStaminaChanged;
        public event Action OnCurrentStaminaChanged;

        private void Start()
        {
            MaxStamina = _initialMaxStamina;
            CurrentStamina = _initialMaxStamina;
        }

        private void Update()
        {
            //debug
            if (UnityEngine.Input.GetKeyDown(KeyCode.F))
            {
                ConsumeStamina(10);
            }

            if (CurrentStamina < _initialMaxStamina && _staminaRegenDelayCO == null) //Stamina is not full and delay timer is not running
            {
                RestoreStamina(_staminaRegenRate * Time.deltaTime);
            }
        }

        private IEnumerator StaminaRegenDelayCO()
        {
            yield return WaitHandler.GetWaitForSeconds(_staminaRegenDelay);

            _staminaRegenDelayCO = null;
        }

        public void ConsumeStamina(float amount)
        {
            CurrentStamina -= amount;
            OnCurrentStaminaChanged?.Invoke();

            if (_staminaRegenDelayCO != null)
            {
                StopCoroutine(_staminaRegenDelayCO);
                _staminaRegenDelayCO = null;
            }

            _staminaRegenDelayCO = StartCoroutine(StaminaRegenDelayCO());
        }

        public void RestoreStamina(float amount)
        {
            CurrentStamina += amount;
            OnCurrentStaminaChanged?.Invoke();
        }
    }
}