using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class Room : MonoBehaviour
    {
        [Header(GameData.SETTINGS)]
        [SerializeField] private ERoomShape _roomShape;

    }
}