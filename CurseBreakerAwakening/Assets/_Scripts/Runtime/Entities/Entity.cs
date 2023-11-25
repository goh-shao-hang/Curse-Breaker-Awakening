using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class Entity : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [field: SerializeField] public SO_EntityData EntityData;

        //TODO temporary
        public void Destroy(float delay)
        {
            Destroy(gameObject, delay);
        }
    }
}