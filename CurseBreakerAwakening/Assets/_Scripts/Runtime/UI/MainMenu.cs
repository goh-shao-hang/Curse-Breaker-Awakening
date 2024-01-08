using CBA;
using CBA.Core;
using DG.Tweening;
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
    //TODO
    [SerializeField] private SceneField _empty;

    [Header("Title")]
    [SerializeField] private Image _title;
    [SerializeField] private float _titleFinalYPosition = 200f;
    [SerializeField] private float _titleFinalScale = 0.9f;
    [SerializeField] private float _titleTweenDuration = 1f;

    [Header("Buttons")]
    [SerializeField] private ButtonScrollList _buttons;

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
        SceneTransitionManager.Instance.LoadSceneWithTransition(_empty, false);

        //TODO
        GameManager.Instance.StartRun(5f);
    }

    public void ShowOptions()
    {
        Debug.Log("ShowOptions");
    }

    public void ShowCredits()
    {
        Debug.Log("ShowCredits");
    }

    public void QuitGame()
    {
        Debug.LogWarning("QUIT");
        Application.Quit();
    }

}
