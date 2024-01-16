using CBA.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [field: SerializeField] public PlayerController PlayerController;
    [field: SerializeField] public PlayerCameraController PlayerCameraController;
    [field: SerializeField] public PlayerHUDManager PlayerHUDManager;

    public void ActivateComponents(bool activate)
    {
        PlayerController.gameObject.SetActive(activate);
        PlayerController.MovementModule.SetVelocity(Vector3.zero);

        PlayerCameraController.gameObject.SetActive(activate);
        PlayerHUDManager.gameObject.SetActive(activate);
    }
}

