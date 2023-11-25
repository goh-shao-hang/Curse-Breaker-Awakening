using GameCells;
using GameCells.Utilities;
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
            GameEventsManager.Instance.CameraShake(cameraShakeData.Direction, cameraShakeData.Strength);
            //_cameraShakeGameEvent?.Invoke(cameraShakeData);
            //GameEventsManager.Instance?.CameraShake(cameraShakeData.Direction, cameraShakeData.Strength);
        }

        //Shake using default values
        /*public void CameraShakeDefault()
        {
            _cameraShakeGameEvent?.Invoke(Vector3.one, 0.3f);
            //GameEventsManager.Instance?.CameraShake(Vector3.one, 0.3f);
        }*/
    }
}