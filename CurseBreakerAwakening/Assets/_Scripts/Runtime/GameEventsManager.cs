using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells
{
    public class GameEventsManager : Singleton<GameEventsManager>
    {
        public event Action<Vector3, float> OnCameraShakeEvent;

        private void Awake()
        {
            LeanTween.reset();
        }

        public void CameraShake()
        {
            OnCameraShakeEvent?.Invoke(Vector3.one, 0.1f);
        }

        public void CameraShake(Vector3 direction, float strength)
        {
            OnCameraShakeEvent?.Invoke(direction, strength);
        }
    }
}