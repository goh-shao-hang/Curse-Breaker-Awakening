using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Utilities
{
    [CreateAssetMenu(menuName = "Data/Camera Shake Data", fileName = "CameraShakeData")]
    public class SO_CameraShakeData : ScriptableObject
    {
        public int Direction;
        public float Strength;
    }
}