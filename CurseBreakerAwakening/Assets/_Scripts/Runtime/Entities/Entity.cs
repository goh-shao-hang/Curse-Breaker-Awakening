using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CBA.Entities
{
    public class Entity : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [field: SerializeField] public SO_EntityData EntityData;

        //TODO
        [field: SerializeField] public SO_GlobalPosition _playerPos;

        public event Action OnDeath;

        public void Die()
        {
            OnDeath?.Invoke();
        }
    }
}