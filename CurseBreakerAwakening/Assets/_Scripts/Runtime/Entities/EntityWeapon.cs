using CBA.Entities.Player.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class EntityWeapon : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Collider _collider;
        [SerializeField] private HealthModule _healthModule;
        [SerializeField] private CombatAnimationEventHander _combatAnimationEventHander;

        /*[Header(GameData.SETTINGS)]
        [SerializeField] private bool _destroyAfterDetach = true;
        [SerializeField] private float _destroyDelay = 3f;*/

        private float _currentAttackDamage;

        private Rigidbody _rigidbody;

        private void OnEnable()
        {
            _combatAnimationEventHander.OnActivateHitboxEvent += EnableHitbox;
            _combatAnimationEventHander.OnDeactivateHitboxEvent += DisableHitbox;

            _healthModule.OnHealthDepleted.AddListener(OnDeath);
        }

        private void OnDisable()
        {
            _combatAnimationEventHander.OnActivateHitboxEvent -= EnableHitbox;
            _combatAnimationEventHander.OnDeactivateHitboxEvent -= DisableHitbox;

            _healthModule.OnHealthDepleted.RemoveListener(OnDeath);
        }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        /*private void Start()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();

            _collider.enabled = false;
            _collider.isTrigger = true;
            _rigidbody.isKinematic = true;
        }*/

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
                other.GetComponent<IDamageable>()?.TakeDamage(_currentAttackDamage);
            }
        }

        public void OnDeath()
        {
            _collider.enabled = false;

            /*transform.SetParent(null);
            _collider.isTrigger = false;
            _rigidbody.isKinematic = false;

            if (!_destroyAfterDetach)
                return;

            Destroy(gameObject, _destroyDelay);*/
        }
    }
}