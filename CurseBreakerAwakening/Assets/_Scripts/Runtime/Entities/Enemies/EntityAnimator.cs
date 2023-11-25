using CBA;
using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private HealthModule _healthModule;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _healthModule.OnHealthChanged.AddListener(PlayHitAnimation);
    }

    private void OnDisable()
    {
        _healthModule.OnHealthChanged.RemoveListener(PlayHitAnimation);
    }

    private void PlayHitAnimation()
    {
        _animator.SetTrigger(GameData.HIT_HASH);
    }
}
