using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CBA.Entities
{
    public class Entity : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [field: SerializeField] public SO_EntityData EntityData;

        //TODO
        [FormerlySerializedAs("_playerPos")] [field: SerializeField] public SO_GlobalPosition PlayerPos;

        public UnityEvent OnDeath;

        public void Die()
        {
            OnDeath?.Invoke();
        }
    }
}