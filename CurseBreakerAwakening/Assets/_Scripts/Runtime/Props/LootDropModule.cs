using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CBA
{
    public class LootDropModule : MonoBehaviour
    {
        [Header(GameData.SETTINGS)]
        [SerializeField] private LootItem[] _lootTable;
        [SerializeField] private Vector3 _dropOffset = Vector3.up;
        [SerializeField] private float _launchForce = 2f;
        [SerializeField] private int _minDrops = 5;
        [SerializeField] private int _maxDrops = 10;

        private LootManager _lootManager;
        private LootManager lootManager => _lootManager ??= LootManager.Instance;

        private Vector3 _lootLaunchForce;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                Drop();
            }
        }

        public async void Drop()
        {
            if (lootManager == null || _lootTable.Length < 1)
                return;

            int dropAmount = Random.Range(_minDrops, _maxDrops + 1);

            for (int i = 0; i < dropAmount; i++)
            {
                _lootLaunchForce.Set(Random.Range(-_launchForce, _launchForce), Random.Range(-_launchForce, _launchForce), Random.Range(-_launchForce, _launchForce));

                Loot loot = GetRandomLootFromTable();

                lootManager.SpawnLoot(loot, transform.position + _dropOffset, Quaternion.identity)
                    .Initialize(_lootLaunchForce, transform.position + _dropOffset, lootManager.GetPool(loot.GetLootType()));

                await Task.Delay((int)(GameData.LOOT_SPAWN_DELAY * 1000f));
            }
        }

        private Loot GetRandomLootFromTable()
        {
            float totalChance = 0f;
            foreach (LootItem item in _lootTable)
            {
                totalChance += item.DropChance;
            }

            float chance = Random.Range(0f, totalChance);
            float cumulativeChance = 0f;

            for (int i = 0; i < _lootTable.Length; i++)
            {
                cumulativeChance += _lootTable[i].DropChance;

                if (chance <= cumulativeChance)
                {
                    return _lootTable[i].lootPrefab;
                }
            }

            //Return last one. This line will never be reached since the for loop will return latest at the last item but has to be here to prevent errors.
            return _lootTable[_lootTable.Length].lootPrefab;
        }

        [Serializable]
        public class LootItem
        {
            [field: SerializeField] public Loot lootPrefab { get; private set; }

            [Range(0, 1)]
            [SerializeField] private float _dropChance;

            public float DropChance => _dropChance;
        }
    }
}