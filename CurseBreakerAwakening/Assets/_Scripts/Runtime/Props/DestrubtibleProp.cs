using CBA.Core;
using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace CBA
{
    public class DestrubtibleProp : MonoBehaviour, IDamageable
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private GameObject _destroyedPieces;
        [SerializeField] private AudioEmitter _audioEmitter;

        [Header("Optional")]
        [SerializeField] private GrabbableObject _grabbableObject;
        [SerializeField] private LootDropModule _lootDropModule;

        [Header(GameData.SETTINGS)]
        [SerializeField] private bool _randomScaleOnStart;
        [SerializeField] private float _minScale = 1;
        [SerializeField] private float _maxScale = 1.5f;
        [SerializeField] private string _destroyedSfxName = "PropsDestroyed_Wood";
        [SerializeField] private bool _destroyedWhenThrown;
        [SerializeField] private bool _destroyAfterDelay = true;

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

        private void OnEnable()
        {
            if (_grabbableObject == null)
                return;

            _grabbableObject.OnThrowCollision.AddListener(() => TakeDamage(1f));
        }

        private void OnDisable()
        {
            if (_grabbableObject == null)
                return;

            _grabbableObject.OnThrowCollision.RemoveListener(() => TakeDamage(1f));
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.B))
            {
                _audioEmitter?.PlayOneShotSfx(_destroyedSfxName);
            }
        }

        public void TakeDamage(float amount)
        {
            _destroyedPieces.SetActive(true);
            _destroyedPieces.transform.SetParent(null);

            foreach (var rigidbody in _destroyedPiecesRigidbodies)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, Random.Range(_upForceMin, _upForceMax), ForceMode.Impulse);

                if (_destroyAfterDelay)
                    Destroy(rigidbody.gameObject, GameData.PROPS_DESTROYED_DELAY);
            }

            _audioEmitter.transform.SetParent(null);
            _audioEmitter?.PlayOneShotSfx(_destroyedSfxName);

            _lootDropModule?.Drop();

            Destroy(gameObject);
        }
    }
}