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
    [SerializeField] protected float _cooldown = 10f;
    [field: SerializeField] public bool CanMoveWhileCasting = false;

    public abstract int SpellAnimationHash { get; }
    public event Action OnSpellCompleted;

    public virtual bool IsAvailable => !this.isOnCooldown; //Condition can be overriden

    protected bool isOnCooldown = false;

    public virtual void Cast() 
    {

    }

    public virtual void Complete() 
    {
        StartCooldown();
        OnSpellCompleted?.Invoke();
    }

    protected virtual void StartCooldown()
    {
        isOnCooldown = true;
        DOVirtual.DelayedCall(_cooldown, () => isOnCooldown = false);
    }

}
