using CBA;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCells.Utilities
{
    public class RagdollController : MonoBehaviour
    {
        public enum StartMode
        {
            Animator,
            Ragdoll
        }

        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _ragdollRootTransform;

        [Header(GameData.SETTINGS)]
        [SerializeField] private StartMode _startMode;
        [SerializeField] private LayerMask _layersToExcludeWhenSearching;


        private Rigidbody[] _rigidbodies;
        private Joint[] joints;
        private Collider[] _colliders;

        private void Awake()
        {
            _rigidbodies = _ragdollRootTransform.GetComponentsInChildren<Rigidbody>().Where(x => ((_layersToExcludeWhenSearching.value) & (1 << x.gameObject.layer)) == 0).ToArray();

            List<Collider> colliders = new List<Collider>();
            foreach (var rigidbody in _rigidbodies)
            {
                colliders.Add(rigidbody.GetComponent<Collider>());
            }
            _colliders = colliders.ToArray();

            joints = GetComponentsInChildren<Joint>();

            if (_startMode == StartMode.Animator)
            {
                EnableAnimator();
            }
            else
            {
                EnableRagdoll();
            }
        }

        public void EnableAnimator()
        {
            _animator.enabled = true;

            foreach (Rigidbody rigidbody in _rigidbodies)
            {
                rigidbody.detectCollisions = false;
                rigidbody.isKinematic = true;
            }

            foreach (Collider collider in _colliders)
            {
                collider.enabled = false;
            }

            foreach (Joint joint in joints)
            {
                joint.enableCollision = false;
            }
        }

        public void EnableRagdoll()
        {
            _animator.enabled = false;

            foreach (Rigidbody rigidbody in _rigidbodies)
            {
                rigidbody.detectCollisions = true;
                rigidbody.isKinematic = false;
            }

            foreach (Collider collider in _colliders)
            {
                collider.enabled = true;
            }

            foreach (Joint joint in joints)
            {
                joint.enableCollision = true;
            }
        }
    }
}