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

        public void CameraShake(Vector3 direction, float strength)
        {
            OnCameraShakeEvent?.Invoke(direction, strength);
        }
    }
}