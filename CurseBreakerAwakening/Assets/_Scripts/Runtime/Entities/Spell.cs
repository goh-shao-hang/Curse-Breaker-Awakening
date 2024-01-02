using CBA;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [Header(GameData.SETTINGS)]
    [SerializeField] protected float _cooldown = 10f;

    public bool IsAvailable { get; protected set; } = true;

    public virtual void Activate() 
    {
        IsAvailable = false;
    }

    public virtual void Deactivate() { }

    protected virtual void StartCooldown()
    {
        DOVirtual.DelayedCall(_cooldown, () => IsAvailable = true);
    }

}
