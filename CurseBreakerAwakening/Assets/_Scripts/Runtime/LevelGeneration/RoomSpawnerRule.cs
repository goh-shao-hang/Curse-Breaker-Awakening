using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    [Serializable]
    public class RoomSpawnerRule
    {
        [HideInInspector] public string ruleName;

        public ERoomType roomType;
        public int MinSpawnIndex;
        public int MaxSpawnIndex;
        [Range(0, 1)]
        public float SpawnChance = 1;

        public void UpdateName()
        {
            ruleName = roomType.ToString();
        }
    }
}