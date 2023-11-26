using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CBA.Entities
{
    public class NavMeshAgentModule : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private SO_GlobalPosition _defaultTargetPosition;

        [Header(GameData.SETTINGS)]
        [SerializeField] private bool _overrideAgentRotation = true;
        //The reason to override the navmeshagent's rotation is to create a dynamic rotation system that rotates faster if the facing direction too far from target angle.
        [SerializeField] private float _overrideRotationFactor = 0.5f;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speedDampTime = 0.5f;

        private SO_GlobalPosition _targetPosition;

        private void Awake()
        {
            _targetPosition = _defaultTargetPosition;

            _navMeshAgent.updateRotation = !_overrideAgentRotation;
        }

        private void Update()
        {
            _navMeshAgent.SetDestination(_targetPosition.Value);

            if (_overrideAgentRotation)
            {
                Vector3 targetDirection = (_targetPosition.Value - _navMeshAgent.transform.position).normalized;
                targetDirection.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(targetDirection);
                _navMeshAgent.transform.rotation = Quaternion.Slerp(_navMeshAgent.transform.rotation, targetRot, _overrideRotationFactor * Time.deltaTime);
            }

            if (_animator != null)
            {
                _animator.SetBool(GameData.ISMOVING_HASH, _navMeshAgent.velocity.sqrMagnitude > 0.01f);

               /* Vector3 normalizedSpeed = transform.TransformDirection(_navMeshAgent.velocity.normalized);
                _animator.SetFloat(GameData.XVELOCITY_HASH, normalizedSpeed.x, _speedDampTime, Time.deltaTime);
                _animator.SetFloat(GameData.ZVELOCITY_HASH, normalizedSpeed.z, _speedDampTime, Time.deltaTime);*/
            }
        }

        public void SetDestination()
        {

        }

        public void Enable()
        {
            _navMeshAgent.enabled = true;
            this.enabled = true;
        }

        public void Disable()
        {
            _navMeshAgent.enabled = false;
            this.enabled = false;
        }
    }
}