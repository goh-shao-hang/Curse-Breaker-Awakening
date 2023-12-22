using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class DestrubtibleProp : MonoBehaviour, IDamageable
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private GameObject _destroyedPieces;

        [Header(GameData.SETTINGS)]
        [SerializeField] private bool _randomScaleOnStart;
        [SerializeField] private float _minScale = 1;
        [SerializeField] private float _maxScale = 1.5f;

        [Header("Explosion")]
        [SerializeField] private float _explosionForce = 2.0f;
        [SerializeField] private float _explosionRadius = 5.0f;
        [SerializeField] private float _upForceMin = 0.0f;
        [SerializeField] private float _upForceMax = 0.5f;

        private Rigidbody[] _destroyedPiecesRigidbodies;

        private void Awake()
        {
            _destroyedPiecesRigidbodies = _destroyedPieces.GetComponentsInChildren<Rigidbody>();
        }

        private void Start()
        {
            if (!_randomScaleOnStart)
                return;

            transform.localScale = Vector3.one * Random.Range(_minScale, _maxScale);
        }

        public void TakeDamage(float amount)
        {
            _destroyedPieces.SetActive(true);
            _destroyedPieces.transform.SetParent(null);

            foreach (var rigidbody in _destroyedPiecesRigidbodies)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, Random.Range(_upForceMin, _upForceMax), ForceMode.Impulse);
                Destroy(rigidbody.gameObject, 5f    );
            }

            Destroy(gameObject);
        }
    }
}