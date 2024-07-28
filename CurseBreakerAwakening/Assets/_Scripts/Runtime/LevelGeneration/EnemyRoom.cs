using CBA.Core;
using CBA.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class EnemyRoom : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Transform _enemiesHolder;
        private List<Entity> _enemies;

        private Room _room;
        private bool _roomCleared = false;

        private int _initialEnemyCount;

        public event Action<int> OnEnemyCountChanged;

        private void Awake()
        {
            _room = GetComponent<Room>();
            _room.OnPlayerEnterRoom += OnPlayerFirstEnter;

            _enemies = _enemiesHolder.GetComponentsInChildren<Entity>().ToList();
            _initialEnemyCount = _enemies.Count;

            if (_enemies.Count < 1)
            {
                Debug.LogWarning("No Enemies Found. This room will be treated as a cleared room.");
                _roomCleared = true;
                return;
            }

            foreach (Entity enemy in _enemies)
            {
                enemy.OnDeath.AddListener(OnAnyEnemyDeath);
            }
        }

        private void OnAnyEnemyDeath(Entity deadEnemy)
        {
            _enemies.Remove(deadEnemy);
            deadEnemy.OnDeath.RemoveListener(OnAnyEnemyDeath);

            float remainingEnemyPercentage = (float)_enemies.Count / (float)_initialEnemyCount;
            if (remainingEnemyPercentage > 0.66f)
            {
                AudioManager.Instance.SetSnapshot(AudioManager.Instance.Snapshot_Combat3, 1.5f);
            }
            else if (remainingEnemyPercentage > 0.33f)
            {
                AudioManager.Instance.SetSnapshot(AudioManager.Instance.Snapshot_Combat2, 1.5f);
            }
            else if (remainingEnemyPercentage > 0f)
            {
                AudioManager.Instance.SetSnapshot(AudioManager.Instance.Snapshot_Combat1, 1.5f);
            }
            else
            {
                _roomCleared = true;
                _room.UnlockRoom();
                AudioManager.Instance.CrossFadeBGM("ExplorationTheme_1", 2f, true);

                AudioManager.Instance.SetSnapshot(AudioManager.Instance.Snapshot_Default);
            }
        }

        private void OnPlayerFirstEnter()
        {
            if (_roomCleared)
                return;

            _room.LockRoom();

            OnEnemyCountChanged?.Invoke(_enemies.Count);

            AudioManager.Instance.CrossFadeBGM("BattleTheme_1", 2f, true);

            //TODO adaptive audio
            AudioManager.Instance.SetSnapshot(AudioManager.Instance.Snapshot_Combat3);
        }
        
    }
}