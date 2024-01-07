using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [SerializeField] private Loot _coinPrefab;

    public ObjectPool<Loot> CoinPool { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        CoinPool = new ObjectPool<Loot>(_coinPrefab, 50);
    }

    public Loot SpawnCoin(Vector3 position, Quaternion rotation)
    {
        return CoinPool.GetFromPool(position, rotation);
    }
}
