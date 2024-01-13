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
    //Commonly accessed
    [field: SerializeField] public Animator Animator;
    [field: SerializeField] public SkinnedMeshRenderer MeshRenderer;
    [field: SerializeField] public EnemyHurtbox Hurtbox;
    

#if UNITY_EDITOR
    [ContextMenu("Quick Setup")]
    public void QuickSetup()
    {
        Animator ??= GetComponentInChildren<Animator>();
        MeshRenderer ??= GetComponentInChildren<SkinnedMeshRenderer>();
        Hurtbox ??= GetComponentInChildren<EnemyHurtbox>();
    }
#endif
}
