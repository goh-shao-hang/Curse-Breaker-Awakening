using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTester : MonoBehaviour
{
    [SerializeField] private CanvasGroup _mapCanvas;

    private bool _mapOpened = false;

    private void Start()
    {
        _mapCanvas.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenMap(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            OpenMap(false);
        }
    }

    private void OpenMap(bool open)
    {
        _mapOpened = open;
        Helper.LockAndHideCursor(!_mapOpened);
        _mapCanvas.alpha = _mapOpened ? 1 : 0;
    }

}
