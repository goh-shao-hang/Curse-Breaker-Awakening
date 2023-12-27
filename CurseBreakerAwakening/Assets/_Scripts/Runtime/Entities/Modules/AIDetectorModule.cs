using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class AIDetectorModule : Module
    {
        [SerializeField] private float _detectionRange;
        [SerializeField] private LayerMask _targetLayer;

        public bool TargetDetected => Physics.CheckSphere(transform.position, _detectionRange, _targetLayer);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
        }
    }
}