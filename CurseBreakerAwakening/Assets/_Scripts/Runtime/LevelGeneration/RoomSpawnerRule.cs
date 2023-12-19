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
        public int[] SpawnIndex;
        public bool IsOptional = false;

        public void UpdateName()
        {
            ruleName = roomType.ToString();
        }
    }
}