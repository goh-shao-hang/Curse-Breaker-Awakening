using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace CBA.Entities
{
    public class Spell_Blast : Spell
    {
        [SerializeField] private Entity _owner;
        [SerializeField] private Transform _vfxPosition;
        [SerializeField] private GameObject _startVFX;
        [SerializeField] private GameObject _completeVFX;
        [SerializeField] private float _blastRadius;
        [SerializeField] private float _damage = 30f;
        [SerializeField] private LayerMask _targetLayers;
        [SerializeField] private LayerMask _destroyLayers;
        [SerializeField] private float _completeDelay = 0.5f;

        private Collider[] _colliderCache = new Collider[10];

#if UNITY_EDITOR
        [Header(GameData.DEBUG)]
        [SerializeField] private bool _visualize = true;
#endif

        public override int SpellAnimationHash => GameData.CASTBLAST_HASH;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_visualize)
                return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _blastRadius);
        }
#endif

        public override void Cast()
        {
            base.Cast();

            if (_startVFX != null)
            {
                Instantiate(_startVFX, _vfxPosition.position, Quaternion.identity);
            }
        }

        public async override void Complete()
        {
            if (_completeVFX != null)
            {
                Instantiate(_completeVFX, _vfxPosition.position, Quaternion.identity);
            }

            int caughtInBlast = Physics.OverlapSphereNonAlloc(transform.position, _blastRadius, _colliderCache, _targetLayers);
            for (int i = 0; i < caughtInBlast; i++)
            {
                if (_colliderCache[i].gameObject == _owner.gameObject)
                    continue;

                _colliderCache[i].GetComponent<IDamageable>()?.TakeDamage(_damage);
            }

            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 1.5f, _destroyLayers))
            {
                Debug.LogError(hit.collider.gameObject.name);
                hit.collider.gameObject.SetActive(false);
            }

            await Task.Delay((int)(_completeDelay * 1000));

            base.Complete();
        }
    }
}