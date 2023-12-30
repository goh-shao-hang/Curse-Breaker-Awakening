using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace GameCells.Utilities
{
    public class AnimationRiggingController : MonoBehaviour
    {
        [SerializeField] private AINavigationModule _navigationModule;
        [SerializeField] private Rig _mainRig;
        [SerializeField] private Transform _target;

        private SO_GlobalPosition _targetPosition;

        private void Start()
        {
            SetWeight(0f);
        }

        private void OnEnable()
        {
            _navigationModule.OnTargetSet += SetTargetPosition;
            _navigationModule.OnTargetRemoved += RemoveTargetPosition;
        }

        private void OnDisable()
        {
            _navigationModule.OnTargetSet -= SetTargetPosition;
            _navigationModule.OnTargetRemoved -= RemoveTargetPosition;
        }

        private void Update()
        {
            if (_targetPosition == null)
                return;

            _target.transform.position = _targetPosition.Value;
        }

        public void SetWeight(float weight)
        {
            _mainRig.weight = weight;
        }

        public void SetTargetPosition()
        {
            _targetPosition = _navigationModule.FollowPosition;
            SetWeight(1f);
        }

        public void RemoveTargetPosition()
        {
            _targetPosition = null;
            SetWeight(0f);
        }
    }
}