using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Spell_SingleProjectile : Spell
    {
        [SerializeField] private Entity _entity;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private LayerMask _targetLayers;

        private ObjectPool<Projectile> _projectilePool;

        public Queue<Projectile> ProjectilePool { get; private set; } = new Queue<Projectile>();

        private void Awake()
        {
            _projectilePool = new ObjectPool<Projectile>(_projectilePrefab, 1);
            _projectilePool.GrowPool();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
                StartCasting();
        }

        public override void StartCasting()
        {
            base.StartCasting();
        }

        public override void OnCastComplete()
        {
            base.OnCastComplete();

            Projectile projectile = _projectilePool.GetFromPool().InitializeWithDelay(this._entity.gameObject, _projectilePool, _spawnTransform.position, _entity.PlayerPos, _targetLayers, _spawnTransform);
        }
    }
}