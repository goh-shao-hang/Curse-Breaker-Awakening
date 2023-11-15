using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player.Weapons
{
    public class WeaponAnimationEventHander : MonoBehaviour
    {
        public event Action OnActivateHitboxEvent;
        public event Action OnDeactivateHitboxEvent;
        public event Action OnAllowNextComboEvent;
        public event Action<int> OnCameraShakeEvent;

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

        public void CameraShake(int direction)
        {
            OnCameraShakeEvent?.Invoke(direction);
        }
    }
}