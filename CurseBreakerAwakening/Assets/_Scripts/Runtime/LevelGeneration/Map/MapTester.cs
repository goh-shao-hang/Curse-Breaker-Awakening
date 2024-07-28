using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTester : MonoBehaviour
{
    [SerializeField] private CanvasGroup _mapCanvas;

    private bool _mapOpened = false;

    private void Start()
    {
        _mapCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO better input check
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenMap(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            OpenMap(false);
        }
        else if (Gamepad.current != null && Gamepad.current.selectButton.wasPressedThisFrame)
        {
            OpenMap(!_mapOpened);
        }
    }

    private void OpenMap(bool open)
    {
        _mapOpened = open;
        Helper.LockAndHideCursor(!_mapOpened);
        _mapCanvas.gameObject.SetActive(open);
    }

}
