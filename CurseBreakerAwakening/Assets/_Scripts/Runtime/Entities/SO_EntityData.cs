using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "Data/Entity Data")]
    public class SO_EntityData : ScriptableObject
    {
        public float Health = 10;
        public float Attack = 5;
        public float Speed = 5;
    }
}