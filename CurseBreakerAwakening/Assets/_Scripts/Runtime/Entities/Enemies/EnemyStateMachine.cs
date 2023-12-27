using CBA.Entities;
using CBA;
using GameCells.StateMachine;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CBA.Modules;

public class EnemyStateMachine : StateMachine
{
    private Entity _entity;
    protected Entity entity => _entity ??= GetComponent<Entity>();

    [Header(GameData.MODULES)]
    [field: SerializeField] public ModuleManager ModuleManager;
    [field: SerializeField] public Animator Animator;
    [field: SerializeField] public EnemyHurtbox Hurtbox;
    
}
