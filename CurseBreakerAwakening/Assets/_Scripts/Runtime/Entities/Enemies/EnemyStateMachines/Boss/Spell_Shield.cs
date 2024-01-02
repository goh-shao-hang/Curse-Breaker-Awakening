using DG.Tweening;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CBA.Entities
{
    public class Spell_Shield : Spell
    {
        [SerializeField] private float _shieldDuration;

        private const float _shieldTween = 1f;

        private MeshRenderer _meshRenderer;
        private Material _shieldMaterial;
        private Collider _shieldCollider;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _shieldMaterial = _meshRenderer.material;
            _shieldCollider = GetComponent<Collider>();
        }

        private void Start()
        {
            _shieldCollider.enabled = false;
            _shieldMaterial.SetFloat(GameData.DISSOLVE, 0);
        }

        public override void Cast()
        {
            base.Cast();

            _shieldCollider.enabled = true;
            _shieldMaterial.DOFloat(1, GameData.DISSOLVE, _shieldTween).SetEase(Ease.OutSine);

            StartCoroutine(ShieldDurationCO());
        }

        private IEnumerator ShieldDurationCO()
        {
            yield return WaitHandler.GetWaitForSeconds(_shieldDuration);

            _shieldCollider.enabled = false;
            _shieldMaterial.DOFloat(0, GameData.DISSOLVE, _shieldTween).SetEase(Ease.OutSine);
        }
    }
}