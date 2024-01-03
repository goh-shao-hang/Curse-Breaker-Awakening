using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CBA.Entities
{
    public class Spell_Multishot : Spell
    {
        [SerializeField] private Entity _entity;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private Transform _multishotRoot;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private int _projectileCount = 5;
        [SerializeField] private float _spawnDelay = 0.25f;
        [SerializeField] private LayerMask _targetLayers;

        public override int SpellAnimationHash => GameData.CASTMULTISHOT_HASH;

        private ObjectPool<Projectile> _projectilePool;

        public Queue<Projectile> ProjectilePool { get; private set; } = new Queue<Projectile>();

        private void Awake()
        {
            _projectilePool = new ObjectPool<Projectile>(_projectilePrefab, _projectileCount);
            _projectilePool.GrowPool();
        }

        public async override void Cast()
        {
            base.Cast();

            Vector3 offset;

            for (int i = 0; i < _projectileCount; i++)
            {
                offset = Vector3.zero;
                offset.x = Mathf.Lerp(-_projectileCount * 0.5f, _projectileCount * 0.5f, (i / (float)(_projectileCount - 1)));
                offset = transform.TransformDirection(offset);

                Projectile projectile = _projectilePool.GetFromPool().InitializeWithDelay(this._entity.gameObject, _projectilePool, _spawnTransform.position, _entity.PlayerPos, _targetLayers);
                projectile.transform.DOMove(_multishotRoot.position + offset, 0.5f).SetEase(Ease.OutSine);

                await Task.Delay((int)(_spawnDelay * 1000f));
            }
        }

    }
}