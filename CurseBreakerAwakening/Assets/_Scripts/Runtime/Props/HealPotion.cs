using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class HealPotion : Loot
    {
        [SerializeField] private float _healAmount = 30f;

        protected override void OnCollected(GameObject _playerGameObject)
        {
            HealthModule healthModule = _playerGameObject.GetComponentInChildren<HealthModule>();
            healthModule.RestoreHealth(_healAmount);
        }
    }
}