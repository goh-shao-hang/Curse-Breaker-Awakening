using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    [CreateAssetMenu(menuName = "Data/Loot Data")]
    public class SO_LootData : ScriptableObject
    {
        [field: SerializeField] public Loot _lootPrefab { get; private set; }

        [Range(0, 1)]
        [SerializeField] private float _dropChance;

        public float DropChance => _dropChance;
    }
}