using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class EntityWeapon : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _healthModule;

        [Header(GameData.SETTINGS)]
        [SerializeField] private bool _destroyAfterDetach = true;
        [SerializeField] private float _destroyDelay = 3f;

        private Collider _collider;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _healthModule.OnHealthDepleted.AddListener(Detach);

            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _collider.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void Detach()
        {
            transform.SetParent(null);
            _collider.enabled = true;
            _rigidbody.isKinematic = false;

            if (!_destroyAfterDetach)
                return;

            Destroy(gameObject, _destroyDelay);
        }
    }
}