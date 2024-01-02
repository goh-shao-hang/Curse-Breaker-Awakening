using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace CBA.Modules
{
    public class AINavigationModule : Module
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private NavMeshAgent _navMeshAgent;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;
        
        [Header(GameData.SETTINGS)]
        [SerializeField] private bool _overrideAgentRotation = true;
        //The reason to override the navmeshagent's rotation is to create a dynamic rotation system that rotates faster if the facing direction too far from target angle.
        [SerializeField] private float _overrideRotationFactor = 3f;

        [Header("Animation")]
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationType _animationType;

        public enum AnimationType
        {
            Speed,
            BlendTree
        }

        public SO_GlobalPosition FollowPosition { get; private set; } = null;
        public SO_GlobalPosition LookTargetPosition { get; private set; } = null;

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
            if (_animationType == AnimationType.Speed)
                _animator.SetFloat(GameData.SPEED_HASH, _navMeshAgent.velocity.magnitude, GameData.ANIMATIONDAMPTIME, Time.deltaTime);
            else
            {
                Vector3 currentvelocity = _navMeshAgent.velocity;
                currentvelocity = transform.InverseTransformDirection(currentvelocity).normalized;
                _animator.SetFloat(GameData.XVELOCITY_HASH, currentvelocity.x, GameData.ANIMATIONDAMPTIME, Time.deltaTime);
                _animator.SetFloat(GameData.ZVELOCITY_HASH, currentvelocity.z, GameData.ANIMATIONDAMPTIME, Time.deltaTime);
            }
        }

        public void SetLookTarget(SO_GlobalPosition positionToFace)
        {
            LookTargetPosition = positionToFace;
        }

        private void UpdateOverridenRotation()
        {
            Vector3 targetDirection;
            if (LookTargetPosition != null)
            {
                targetDirection = (LookTargetPosition.Value - _navMeshAgent.transform.position);
            }
            else
            {
                targetDirection = (_navMeshAgent.destination - _navMeshAgent.transform.position);
            }

            targetDirection.y = 0;

            if (targetDirection.sqrMagnitude > 0.01f)
            {
                targetDirection = targetDirection.normalized;
                Quaternion targetRot = Quaternion.LookRotation(targetDirection);
                _navMeshAgent.transform.rotation = Quaternion.Slerp(_navMeshAgent.transform.rotation, targetRot, _overrideRotationFactor * Time.deltaTime);
            }
        }

        private void UpdateFollowing()
        {
            _navMeshAgent.SetDestination(FollowPosition.Value);
        }

        public void SetSpeed(float speed)
        {
            _navMeshAgent.speed = speed;
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
            enabled = true;
        }

        public void Disable()
        {
            _navMeshAgent.enabled = false;
            enabled = false;
        }
    }
}