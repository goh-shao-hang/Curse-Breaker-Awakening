using CBA.Core;
using CBA.Entities;
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

        private void Awake()
        {
            _room = GetComponent<Room>();
            _room.OnPlayerEnterRoom += OnPlayerFirstEnter;

            _enemies = _enemiesHolder.GetComponentsInChildren<Entity>().ToList();

            if (_enemies.Count < 1)
            {
                Debug.LogWarning("No Enemies Found. This room will be treated as a cleared room.");
                _roomCleared = true;
                return;
            }

            foreach (Entity enemy in _enemies)
            {
                enemy.OnDeath += () => OnAnyEnemyDeath(enemy);
            }
        }

        private void OnAnyEnemyDeath(Entity deadEnemy)
        {
            _enemies.Remove(deadEnemy);

            if (_enemies.Count <= 0)
            {
                _roomCleared = true;
                _room.UnlockRoom();
                AudioManager.Instance.CrossFadeBGM("ExplorationTheme_1");
            }
        }

        private void OnPlayerFirstEnter()
        {
            if (_roomCleared)
                return;

            _room.LockRoom();

            AudioManager.Instance.CrossFadeBGM("BattleTheme_1");
        }
        
    }
}