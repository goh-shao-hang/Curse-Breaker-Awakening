using CBA.Entities.Player.Weapons;
using CBA.Modules;
using GameCells.Utilities;
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
            GameObject module = transform.Find("Modules").gameObject;

            module.AddComponent<ModuleManager>();
            module.AddComponent<HealthModule>();
            module.AddComponent<GuardModule>();
            module.AddComponent<AINavigationModule>();
        }

        [ContextMenu("Setup Humanoid Mesh Components")]
        public void SetupHumanoidMeshComponents()
        {
            GameObject mesh = transform.Find("Mesh").gameObject;

            mesh.AddComponent<CharacterOutlineController>();
            mesh.AddComponent<RagdollController>();
            mesh.AddComponent<CombatAnimationEventHander>();
            mesh.AddComponent<CharacterOutlineController>();
            mesh.AddComponent<AnimationRiggingController>();

        }
    }
}