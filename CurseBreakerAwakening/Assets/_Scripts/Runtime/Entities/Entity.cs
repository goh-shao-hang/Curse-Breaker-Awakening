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
        [field: SerializeField] public Animator _animator;

        //TODO temporary
        public void Destroy(float delay)
        {
            Destroy(gameObject, delay);
        }
    }
}