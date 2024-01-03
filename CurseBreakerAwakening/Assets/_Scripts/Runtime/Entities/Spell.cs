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
    [field: SerializeField] public bool CanMoveWhileCasting = false;

    public abstract int SpellAnimationHash { get; }
    public event Action OnSpellCompleted;

    public bool IsAvailable { get; protected set; } = true;

    public virtual void Cast() 
    {
        IsAvailable = false;
    }

    public virtual void Complete() 
    {
        StartCooldown();
        OnSpellCompleted?.Invoke();
    }

    protected virtual void StartCooldown()
    {
        DOVirtual.DelayedCall(_cooldown, () => IsAvailable = true);
    }

}
