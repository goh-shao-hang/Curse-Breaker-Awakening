using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GlobalPositionWriter : MonoBehaviour
{
    [SerializeField] private SO_GlobalPosition _positionToWrite;

    private void Update()
    {
        _positionToWrite.Set(this.transform.position);
    }
}
