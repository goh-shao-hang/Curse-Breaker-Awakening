using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class Room : MonoBehaviour
    {
        [field: SerializeField] public ERoomShape RoomShape { get; private set; }
    }
}