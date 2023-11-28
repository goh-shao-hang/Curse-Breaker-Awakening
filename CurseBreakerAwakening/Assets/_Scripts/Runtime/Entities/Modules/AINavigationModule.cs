using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CBA.Entities
{
    public class AINavigationModule : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private NavMeshAgent _navMeshAgent;

        [Header(GameData.SETTINGS)]
        [SerializeField] private bool _overrideAgentRotation = true;
        //The reason to override the navmeshagent's rotation is to create a dynamic rotation system that rotates faster if the facing direction too far from target angle.
        [SerializeField] private float _overrideRotationFactor = 0.5f;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speedDampTime = 0.5f;

        private SO_GlobalPosition _followPosition = null;

        private void Awake()
        {
            _navMeshAgent.updateRotation = !_overrideAgentRotation;
        }

        private void Update()
        {
            if (_animator != null)
            {
                UpdateAnimation();
            }

            if (_overrideAgentRotation) //If this is false, rotation is automtically handled by Unity's NavMesh Agent.
            {
                UpdateOverridenRotation();
            }

            if (_followPosition != null)
            {
                UpdateFollowing();
            }
        }

        private void UpdateAnimation()
        {
            _animator.SetBool(GameData.ISMOVING_HASH, _navMeshAgent.velocity.sqrMagnitude > 0.01f);
        }

        private void UpdateOverridenRotation()
        {
            Vector3 targetDirection = (_navMeshAgent.destination - _navMeshAgent.transform.position).normalized;
            targetDirection.y = 0;
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(targetDirection);
                _navMeshAgent.transform.rotation = Quaternion.Slerp(_navMeshAgent.transform.rotation, targetRot, _overrideRotationFactor * Time.deltaTime);
            }
        }

        private void UpdateFollowing()
        {
            _navMeshAgent.SetDestination(_followPosition.Value);
        }


        public void SetOneTimeDestination(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
            _followPosition = null;
        }

        public void SetOneTimeDestination(SO_GlobalPosition position)
        {
            _navMeshAgent.SetDestination(position.Value);
            _followPosition = null;
        }

        public void SetFollowPosition(SO_GlobalPosition position)
        {
            _followPosition = position;
        }

        public void StopFollow()
        {
            _followPosition = null;
            _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
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