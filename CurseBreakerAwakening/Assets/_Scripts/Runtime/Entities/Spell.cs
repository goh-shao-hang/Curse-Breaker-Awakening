using CBA;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [Header(GameData.SETTINGS)]
    [SerializeField] protected float _castTime = 3f;
    [SerializeField] protected float _cooldown = 10f;

    public event Action OnCastCompleted;

    public bool IsAvailable { get; protected set; } = true;

    public virtual void StartCasting() 
    {
        IsAvailable = false;
        DOVirtual.DelayedCall(_castTime, OnCastComplete);
    }

    public virtual void OnCastComplete() 
    {
        OnCastCompleted?.Invoke();
        StartCooldown();
    }

    protected virtual void StartCooldown()
    {
        DOVirtual.DelayedCall(_cooldown, () => IsAvailable = true);
    }

}
