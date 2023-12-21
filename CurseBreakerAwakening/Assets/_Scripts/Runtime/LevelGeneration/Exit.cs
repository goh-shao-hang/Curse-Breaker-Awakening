using CBA;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [field: SerializeField] public EExitDirection Direction { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [SerializeField] private GameObject _lockdownBar;

    public event Action<EExitDirection> OnPlayerExit;

    private const float lockdownBarOpenedYPos = 2.3f;
    private const float lockdownBarLockedYPos = 0f;
    private const float lockTweenDuration = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameData.PLAYER_LAYER_INDEX)
        {
            OnPlayerExit?.Invoke(this.Direction);
            Debug.Log("player entered");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lock();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Unlock();
        }
    }

    public void Lock()
    {
        _lockdownBar.transform.DOMoveY(lockdownBarLockedYPos, lockTweenDuration).SetEase(Ease.OutBounce);
    }

    public void Unlock()
    {
        _lockdownBar.transform.DOMoveY(lockdownBarOpenedYPos, lockTweenDuration).SetEase(Ease.InSine);
    }
}
public enum EExitDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}