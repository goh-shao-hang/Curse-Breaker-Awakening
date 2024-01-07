using CBA;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    private Dictionary<Type, ObjectPool<Loot>> _lootPools = new Dictionary<Type, ObjectPool<Loot>>();

    public Loot SpawnLoot(Loot lootPrefab, Vector3 position, Quaternion rotation)
    {
        Type lootType = lootPrefab.GetLootType();

        if (!_lootPools.ContainsKey(lootType))
        {
            _lootPools.Add(lootType, new ObjectPool<Loot>(lootPrefab, 50));
        }

        return _lootPools[lootType].GetFromPool(position, rotation);
    }

    public ObjectPool<Loot> GetPool(Type poolType)
    {
        if (_lootPools.ContainsKey(poolType))
        {
            return _lootPools[poolType];
        }
        else
        {
            Debug.LogError($"Loot Pools does not contain type {poolType}!");
            return null;
        }
    }
}
