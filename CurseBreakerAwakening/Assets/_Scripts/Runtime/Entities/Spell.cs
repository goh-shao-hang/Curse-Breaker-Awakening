using CBA;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [Header(GameData.SETTINGS)]
    [SerializeField] protected float _cooldown = 10f;

    public bool IsAvailable { get; protected set; } = true;

    public virtual void Activate() { }
    public virtual void Deactivate() { }

    protected virtual void StartCooldown(float cooldown)
    {
        DOVirtual.DelayedCall(cooldown, () => IsAvailable = true);
    }

}
