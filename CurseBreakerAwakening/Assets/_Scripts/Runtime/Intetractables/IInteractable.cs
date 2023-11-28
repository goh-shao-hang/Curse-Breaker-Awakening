using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnSelect();
    public void OnDeselect();
    public void OnInteract();
}
