using CBA.Entities;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class ExplosiveModule : Module
    {
        [Header(GameData.SETTINGS)]
        [SerializeField] private float _explosionDamage = 10f;
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private LayerMask _targetLayers;
        [SerializeField] private GameObject _explosionVfx;
        [SerializeField] private Vector3 _vfxOffset;

        [Header(GameData.DEBUG)]
        [SerializeField] private bool _gizmos;

        private Collider[] _colliderCache = new Collider[10];

        private DamageData _explosionDamageData;

        private void OnDrawGizmos()
        {
            if (!_gizmos)
                return;

            Gizmos.DrawSphere(transform.position, _explosionRadius);
        }

        public void SetTargetLayers(LayerMask targetLayers)
        {
            this._targetLayers = targetLayers;
        }

        public void TriggerExplosion()
        {
            int caughtInExplosion = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, _colliderCache, _targetLayers);
            for (int i = 0; i < caughtInExplosion; i++) 
            {
                _explosionDamageData.Set(_explosionDamage, this.gameObject);
                _colliderCache[i].GetComponent<IDamageable>()?.TakeDamage(_explosionDamageData);
            }

            if (_explosionVfx != null)
            {
                Instantiate(_explosionVfx, transform.position + _vfxOffset, Quaternion.identity);
            }
        }
    }
}