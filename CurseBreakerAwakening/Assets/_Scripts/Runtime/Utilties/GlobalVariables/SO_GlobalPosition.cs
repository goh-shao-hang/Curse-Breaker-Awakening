using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Utilities
{
    [CreateAssetMenu(fileName = "GlobalPosition_", menuName = "Global Varitables/Position")]
    public class SO_GlobalPosition : ScriptableObject
    {
        public Vector3 Value { get; private set; }

        public void Set(Vector3 value)
        {
            this.Value = value;
        }
    }
}