using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CBA
{
    public class LootDropModule : MonoBehaviour
    {
        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector3 _dropOffset = Vector3.up;
        [SerializeField] private float _launchForce = 2f;
        [SerializeField] private int _minCoins = 5;
        [SerializeField] private int _maxCoins = 10;
        [SerializeField] private float _spawnDelay = 0.1f;

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
            if (lootManager == null)
                return;

            int dropAmount = Random.Range(_minCoins, _maxCoins + 1);

            for (int i = 0; i < dropAmount; i++)
            {
                _lootLaunchForce.Set(Random.Range(-_launchForce, _launchForce), Random.Range(-_launchForce, _launchForce), Random.Range(-_launchForce, _launchForce));

                lootManager.SpawnCoin(transform.position + _dropOffset, Quaternion.identity).Initialize(_lootLaunchForce, transform.position + _dropOffset, lootManager.CoinPool);

                await Task.Delay((int)(_spawnDelay * 1000f));
            }
        }

        public class LootType
        {
            [SerializeField] private Loot lootPrefab;
        }
    }
}