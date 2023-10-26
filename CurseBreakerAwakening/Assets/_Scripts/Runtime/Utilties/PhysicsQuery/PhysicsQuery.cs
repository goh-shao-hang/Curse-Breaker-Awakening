using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Utilities
{
    public abstract class PhysicsQuery : MonoBehaviour
    {
        [SerializeField] private bool _visualize = true;
        [SerializeField] protected LayerMask _targetLayers;

        public void SetOffset(Vector3 offset)
        {
            transform.localPosition = offset;
        }

        public abstract bool Hit();

        public abstract void OnVisualize();

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_visualize)
            {
                Gizmos.color = Hit() ? Color.green : Color.red;

                OnVisualize();
            }
        }

#endif

    }
}