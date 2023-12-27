using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CBA.Entities
{
    public class EntityTemplate : MonoBehaviour
    {
        [ContextMenu("Setup Enemy Base Template")]
        public void SetupEnemyBaseTemplate()
        {
            gameObject.AddComponent<Collider>();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<NavMeshAgent>();
            gameObject.AddComponent<Entity>();
            gameObject.AddComponent<EnemyHurtbox>();
            gameObject.AddComponent<GrabbableObject>();
        }

        [ContextMenu("Setup Enemy Modules Template")]
        public void SetupEnemyModulesTemplate()
        {
            gameObject.AddComponent<HealthModule>();
            gameObject.AddComponent<GuardModule>();
            gameObject.AddComponent<AINavigationModule>();
        }
    }
}