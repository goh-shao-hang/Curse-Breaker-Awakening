using CBA.Entities.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public event Action OnSelected;
    public event Action OnDeselected;

    public void OnSelect();
    public void OnDeselect();
    public void OnInteract(PlayerGrabManager playerGrabManager);
}
