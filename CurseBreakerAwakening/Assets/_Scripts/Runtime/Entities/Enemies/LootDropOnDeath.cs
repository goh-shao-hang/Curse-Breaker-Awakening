using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class LootDropOnDeath : MonoBehaviour
    {
        private Entity _entity;
        private LootDropModule _lootDropModule;

        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _lootDropModule = GetComponentInChildren<LootDropModule>();
        }

        private void OnEnable()
        {
            _entity.OnDeath.AddListener(DropLoot);
        }

        private void OnDisable()
        {
            _entity.OnDeath.RemoveListener(DropLoot);
        }

        private void DropLoot(Entity entity)
        {
            _lootDropModule.Drop();
        }
    }
}