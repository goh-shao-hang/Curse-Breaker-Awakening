using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class EntityInfo : MonoBehaviour
    {
        private Entity _entity;

        public string EntityName { get; private set; }
        public HealthModule HealthModule { get; private set; }
        public GuardModule GuardModule { get; private set; }

        private void Awake()
        {
            _entity = GetComponent<Entity>();

            EntityName = _entity.EntityData.EntityName;
            HealthModule = _entity.GetModule<HealthModule>();
            GuardModule = _entity.GetModule<GuardModule>();
        }
    }
}