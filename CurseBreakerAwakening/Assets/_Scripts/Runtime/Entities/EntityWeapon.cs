using CBA.Entities.Player.Weapons;
using CBA.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class EntityWeapon : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Entity _entity;
        [SerializeField] private Collider _collider;
        [SerializeField] private CombatAnimationEventHander _combatAnimationEventHander;

        [Header("Optional")]
        [SerializeField] private GrabbableObject _grabbableObject;
        [SerializeField] private DestrubtibleProp _destructibleProp;

        private float _currentAttackDamage;

        private DamageData _damageData;

        private void Start()
        {
            this._destructibleProp?.SetCanTakeDamage(false);
        }

        private void OnEnable()
        {
            _combatAnimationEventHander.OnActivateHitboxEvent += EnableHitbox;
            _combatAnimationEventHander.OnDeactivateHitboxEvent += DisableHitbox;

            _entity.OnDeath.AddListener(OnDeath);
        }

        private void OnDisable()
        {
            _combatAnimationEventHander.OnActivateHitboxEvent -= EnableHitbox;
            _combatAnimationEventHander.OnDeactivateHitboxEvent -= DisableHitbox;

            _entity.OnDeath.RemoveListener(OnDeath);
        }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public void SetDamage(float damage)
        {
            this._currentAttackDamage = damage;
        }

        public void EnableHitbox()
        {
            _collider.enabled = true;
        }

        public void DisableHitbox()
        {
            _collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == GameData.PLAYER_LAYER_INDEX)
            {
                _damageData.Set(_currentAttackDamage, _entity.gameObject);
                other.GetComponent<IDamageable>()?.TakeDamage(_damageData);
            }
        }

        public void OnDeath(Entity entity)
        {
            _collider.enabled = true;
            _collider.isTrigger = false;

            //eg. set the parent to the parent of the parent, in this case the
            transform.SetParent(LevelManager.Instance != null ? LevelManager.Instance.CurrentRoom.transform : null);

            this._destructibleProp?.SetCanTakeDamage(true);

            if (_grabbableObject != null)
            {
                _grabbableObject.SetIsGrabbable(true);
                _grabbableObject.EnableThrowPhysics(true);
            }
        }
    }
}