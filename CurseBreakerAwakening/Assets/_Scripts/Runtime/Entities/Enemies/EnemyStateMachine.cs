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

    [Header(GameData.DEPENDENCIES)]
    [field: SerializeField] public ModuleManager ModuleManager;
    //Commonly accessed
    [field: SerializeField] public Animator Animator;
    [field: SerializeField] public SkinnedMeshRenderer MeshRenderer;
    [field: SerializeField] public EnemyHurtbox Hurtbox;
    
    public T GetModule<T>() where T: Module
    {
        return ModuleManager.GetModule<T>();
    }

#if UNITY_EDITOR
    [ContextMenu("Quick Setup")]
    public void QuickSetup()
    {
        ModuleManager ??= GetComponentInChildren<ModuleManager>();
        Animator ??= GetComponentInChildren<Animator>();
        MeshRenderer ??= GetComponentInChildren<SkinnedMeshRenderer>();
        Hurtbox ??= GetComponentInChildren<EnemyHurtbox>();
    }
#endif
}
