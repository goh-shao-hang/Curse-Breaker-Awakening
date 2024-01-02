using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CBA.Entities
{
    public class Spell_Shield : Spell
    {
        [SerializeField] private float _deactivateTimer = 5f;

        private const float _shieldTween = 2f;

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

        public override void Activate()
        {
            _shieldCollider.enabled = true;
            _shieldMaterial.DOFloat(1, GameData.DISSOLVE, _shieldTween).SetEase(Ease.OutSine);

            this.IsAvailable = false;

            DOVirtual.DelayedCall(_deactivateTimer, Deactivate);
        }

        public override void Deactivate()
        {
            _shieldCollider.enabled = false;
            _shieldMaterial.DOFloat(0, GameData.DISSOLVE, _shieldTween).SetEase(Ease.OutSine);

            this.StartCooldown(this._cooldown);
        }


    }
}