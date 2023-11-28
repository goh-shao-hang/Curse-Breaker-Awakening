using GameCells.Utilities;
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
        [field: SerializeField] public AINavigationModule NavMeshAgentModule;
        [field: SerializeField] public Animator Animator;

        //TODO
        [field: SerializeField] public SO_GlobalPosition _playerPos;

        //TODO temporary
        public void Destroy(float delay)
        {
            Destroy(gameObject, delay);
        }
    }
}