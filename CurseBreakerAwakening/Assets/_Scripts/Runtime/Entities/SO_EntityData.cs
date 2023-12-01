using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Data/Entity Data")]
    public class SO_EntityData : ScriptableObject
    {
        [Header("Stats")]
        public float Health = 10f;
        public float Attack = 5f;
        public float Speed = 5f;
        public float Guard = 10f;

        [Header(GameData.SETTINGS)]
        public float BaseStunDuration = 5f;
    }
}