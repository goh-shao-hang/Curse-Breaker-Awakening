using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTester : MonoBehaviour
{
    [SerializeField] private CanvasGroup _mapCanvas;

    private void Start()
    {
        _mapCanvas.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _mapCanvas.alpha = _mapCanvas.alpha == 1 ? 0 : 1;
        }
    }
}
