using CBA;
using CBA.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private SceneField _mainMenuScene;
    [SerializeField] private SceneField _emptyScene;
    [SerializeField] private ButtonScrollList _buttons;

    private void Start()
    {
        AudioManager.Instance.PlayBGM("EndingTheme");

        _buttons.DisableButtons();
        _buttons.ShowButtons();
    }

    public void ReturnToMainMenu()
    {
        _buttons.SetInteractable(false);

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);

        SceneTransitionManager.Instance.LoadSceneWithTransition(_mainMenuScene, false);
    }

    public void QuickRestart()
    {
        _buttons.SetInteractable(false);

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);
        SceneTransitionManager.Instance.LoadSceneWithTransition(_emptyScene, false);

        //TODO
        GameManager.Instance.StartRun(5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
