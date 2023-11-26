using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Data/Entity Data")]
    public class SO_EntityData : ScriptableObject
    {
        public float Health = 10f;
        public float Attack = 5f;
        public float Speed = 5f;
        public float Guard = 10f;
    }
}