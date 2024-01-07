using CBA;
using CBA.Core;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [field: SerializeField] public EExitDirection Direction { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }
    [SerializeField] private Collider _lockDownBar; //Collider and visual of blockage is seperated so that the collider can be immediately activated
    [SerializeField] private GameObject _lockdownBarVisual;
    [SerializeField] private AudioEmitter _doorAudioEmitter;

    public event Action<EExitDirection> OnPlayerExit;

    private const float lockdownBarOpenedYPos = 2.3f;
    private const float lockdownBarLockedYPos = 0f;
    private const float lockTweenDuration = 1f;

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
        //TODO
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lock();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Unlock();
        }
#endif
    }

    public void Lock()
    {
        _lockDownBar.enabled = true;
        _lockdownBarVisual.transform.DOLocalMoveY(lockdownBarLockedYPos, lockTweenDuration).SetEase(Ease.OutBounce);

        _doorAudioEmitter?.PlayOneShotSfx("Gate_Close");
    }

    public void Unlock()
    {
        _lockDownBar.enabled = false;
        _lockdownBarVisual.transform.DOLocalMoveY(lockdownBarOpenedYPos, lockTweenDuration).SetEase(Ease.InSine);

        _doorAudioEmitter?.PlayOneShotSfx("Gate_Open");
    }
}
public enum EExitDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}