using CBA.Entities;
using CBA;
using GameCells.StateMachine;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    [Header(GameData.DEPENDENCIES)]
    [field: SerializeField] public Entity _entity;

    [Header(GameData.MODULES)]
    [field: SerializeField] public AINavigationModule NavMeshAgentModule;
    [field: SerializeField] public Animator Animator;
    [field: SerializeField] public EnemyHurtbox Hurtbox;
    [field: SerializeField] public HealthModule HealthModule;
    [field: SerializeField] public GuardModule GuardModule;
    
}
