using CBA;
using CBA.Core;
using DG.Tweening;
using GameCells.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private SceneField _introScene;

    [Header("Title")]
    [SerializeField] private Image _title;
    [SerializeField] private float _titleFinalYPosition = 200f;
    [SerializeField] private float _titleFinalScale = 0.9f;
    [SerializeField] private float _titleTweenDuration = 1f;

    [Header("Buttons")]
    [SerializeField] private ButtonScrollList _buttons;

    [Header("Credits")]
    [SerializeField] private GCUI_Panel _credits;

    private void Start()
    {
        _buttons.DisableButtons();

        _title.rectTransform.DOAnchorPosY(_titleFinalYPosition, _titleTweenDuration).SetEase(Ease.InOutSine);
        _title.rectTransform.DOScale(_titleFinalScale, _titleTweenDuration).SetEase(Ease.InOutSine).OnComplete(_buttons.ShowButtons);
    }

    public void NewGame()
    {
        _buttons.SetInteractable(false);

        //TODO
        //DO THIS WHENEVER LOADING TO OTHER SCENE
        //DOTween.KillAll();

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);
        SceneTransitionManager.Instance.LoadSceneWithTransition(_introScene, true);
    }

    public void QuickStart()
    {
        _buttons.SetInteractable(false);

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");
        AudioManager.Instance?.StopBGM(2f);

        GameManager.Instance.StartRun(5f);
    }

    public void BossTrail()
    {
        _buttons.SetInteractable(false);

        AudioManager.Instance?.PlayGlobalSFX("MainMenu_Confirm");

        GameManager.Instance.StartBossTrail();
    }

    public void ShowOptions()
    {
        SettingsManager.Instance?.ShowSettingsMenu();

        _buttons.DisableButtons();

        SettingsManager.Instance.OnHide += HideOptions;
    }

    private void HideOptions()
    {
        _buttons.ShowButtons();

        SettingsManager.Instance.OnHide -= HideOptions;
    }

    public void ShowCredits()
    {
        _credits.Show();

        _buttons.DisableButtons();
    }

    public void HideCredits()
    {
        _credits.Hide();
        _buttons.ShowButtons();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
