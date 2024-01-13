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
        [field: SerializeField] public ModuleManager ModuleManager;

        //TODO
        [field: SerializeField] public SO_GlobalPosition PlayerPos;

        public UnityEvent<Entity> OnDeath;

        public T GetModule<T>() where T : Module
        {
            return ModuleManager.GetModule<T>();
        }

        public void Die()
        {
            OnDeath?.Invoke(this);
        }

#if UNITY_EDITOR
        [ContextMenu("Quick Setup")]
        public void QuickSetup()
        {
            ModuleManager = GetComponentInChildren<ModuleManager>();
        }
#endif
    }
}