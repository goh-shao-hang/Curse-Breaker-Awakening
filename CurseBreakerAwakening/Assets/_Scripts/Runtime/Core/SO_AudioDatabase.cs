using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CBA.Core
{
    [CreateAssetMenu(menuName = "Data/Audio Database")]
    public class SO_AudioDatabase : ScriptableObject
    {
        [field: SerializeField] public Audio[] Musics { get; private set; }
        [field: SerializeField] public Audio[] SoundEffects { get; private set; }
    }
}