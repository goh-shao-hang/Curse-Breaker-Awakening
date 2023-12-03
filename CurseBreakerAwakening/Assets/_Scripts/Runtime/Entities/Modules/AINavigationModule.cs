using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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

        public SO_GlobalPosition FollowPosition { get; private set; } = null;

        //For effects only eg. animation rigging
        public event Action OnTargetSet;
        public event Action OnTargetRemoved;

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

            if (FollowPosition != null)
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
            _navMeshAgent.SetDestination(FollowPosition.Value);
        }


        public void SetOneTimeDestination(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
            FollowPosition = null;
        }

        public void SetOneTimeDestination(SO_GlobalPosition position)
        {
            _navMeshAgent.SetDestination(position.Value);
            FollowPosition = null;
        }

        public void SetFollowPosition(SO_GlobalPosition position)
        {
            FollowPosition = position;

            OnTargetSet?.Invoke();
        }

        public void StopFollow()
        {
            FollowPosition = null;
            _navMeshAgent.SetDestination(_navMeshAgent.transform.position);

            OnTargetRemoved?.Invoke();
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