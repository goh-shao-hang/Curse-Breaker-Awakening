using GameCells.Utilities;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Canvas _mainCanvas;

    private bool _paused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    public void OpenPauseMenu()
    {
        _paused = !_paused;
        Helper.LockAndHideCursor(!_paused);
        Time.timeScale = _paused ? 0 : 1;

        _mainCanvas.gameObject.SetActive(_paused);
    }
}
