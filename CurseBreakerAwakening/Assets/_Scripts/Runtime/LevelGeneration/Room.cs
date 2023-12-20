using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class Room : MonoBehaviour
    {
        [field: SerializeField] public ERoomShape RoomShape { get; private set; }

        [field: SerializeField] public Exit[] Exits { get; private set; }

        [Serializable]
        public class Exit
        {
            [field: SerializeField] public EDirection Direction { get; private set; }
            [field: SerializeField] public Collider ExitCollider { get; private set; }

            public static float ExitDirectionToRotation(EDirection direction)
            {
                return (float)direction;
            }

            public enum EDirection
            {
                Up = 0,
                Right = 90,
                Down = 180,
                Left = 270
            }
        }
    }
}