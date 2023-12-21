using CBA;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [field: SerializeField] public EExitDirection Direction { get; private set; }
    [field: SerializeField] public Transform SpawnPoint { get; private set; }

    public event Action<EExitDirection> OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameData.PLAYER_LAYER_INDEX)
        {
            OnPlayerExit?.Invoke(this.Direction);
            Debug.Log("player entered");
        }
    }

}
public enum EExitDirection
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}