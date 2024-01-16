using CBA;
using CBA.Entities.Player;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteraction : MonoBehaviour, IInteractable
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private SO_Dialog _enterRangeDialog;
    [SerializeField] private SO_Dialog _interactDialog;
    [SerializeField] private SO_Dialog _exitRangeDialog;
    [SerializeField] private SO_Dialog _attackedDialog;

    public event Action OnSelected;
    public event Action OnDeselected;

    private bool _playedEnterRangeDialog;
    private bool _playedExitRangeDialog;

    private void OnTriggerEnter(Collider other)
    {
        if (_playedEnterRangeDialog)
            return;

        if (((1 << other.gameObject.layer) & GameData.PLAYER_LAYER) == 0)
            return;

        _playedEnterRangeDialog = true;
        UIManager.Instance.StartDialog(_enterRangeDialog);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_playedExitRangeDialog)
            return;

        if (((1 << other.gameObject.layer) & GameData.PLAYER_LAYER) == 0)
            return;

        _playedExitRangeDialog = true;
        UIManager.Instance.StartDialog(_exitRangeDialog);
    }

    public void OnSelect()
    {
        OnSelected?.Invoke();
    }

    public void OnDeselect()
    {
        OnDeselected?.Invoke();
    }

    public void OnInteract(PlayerGrabManager playerGrabManager)
    {
        UIManager.Instance.StartDialog(_interactDialog);
    }


}
