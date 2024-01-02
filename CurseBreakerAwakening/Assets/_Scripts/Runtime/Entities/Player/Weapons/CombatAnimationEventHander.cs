using CBA.Core;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player.Weapons
{
    public class CombatAnimationEventHander : MonoBehaviour
    {
        public event Action OnActivateHitboxEvent;
        public event Action OnDeactivateHitboxEvent;
        public event Action OnAllowNextComboEvent;
        public event Action OnEmitTrail;
        public event Action OnStopEmitTrail;
        public event Action<int> OnSpellCast;
        public event Action<int> OnSpellComplete;

        public void ActivateHitbox()
        {
            OnActivateHitboxEvent?.Invoke();
        }

        public void DeactivateHitbox()
        {
            OnDeactivateHitboxEvent?.Invoke();
        }

        public void AllowNextCombo()
        {
            OnAllowNextComboEvent?.Invoke();
        }

        public void CameraShake(SO_CameraShakeData cameraShakeData)
        {
            GameEventsManager.Instance?.CameraShake(cameraShakeData.Direction, cameraShakeData.Strength);
        }

        public void EmitTrail()
        {
            OnEmitTrail?.Invoke();
        }

        public void StopEmitTrail()
        {
            OnStopEmitTrail?.Invoke();
        }

        public void CastSpell(int spellIndex)
        {
            OnSpellCast?.Invoke(spellIndex);
        }

        public void CompleteSpell(int spellIndex)
        {
            OnSpellComplete?.Invoke(spellIndex);
            Debug.LogError($"{spellIndex} completed!");
        }

        //Shake using default values
        /*public void CameraShakeDefault()
        {
            _cameraShakeGameEvent?.Invoke(Vector3.one, 0.3f);
            //GameEventsManager.Instance?.CameraShake(Vector3.one, 0.3f);
        }*/
    }
}