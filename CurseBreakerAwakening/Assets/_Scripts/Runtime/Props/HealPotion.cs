using CBA.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class HealPotion : Loot
    {
        [SerializeField] private float _healAmount = 30f;

        public override Type GetLootType()
        {
            return this.GetType();
        }

        protected override void OnCollected(GameObject _playerGameObject)
        {
            HealthModule healthModule = _playerGameObject.GetComponentInChildren<HealthModule>();
            healthModule.RestoreHealth(_healAmount);
        }
    }
}