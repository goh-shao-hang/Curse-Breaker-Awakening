using CBA;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] private Image _title;
    [SerializeField] private float _titleFinalYPosition = 200f;
    [SerializeField] private float _titleFinalScale = 0.9f;
    [SerializeField] private float _titleTweenDuration = 1f;

    [Header("Buttons")]
    [SerializeField] private CanvasGroup _buttonsCanvasGroup;
    [SerializeField] private float _buttonsTweenDuration = 1f;

    private Button[] _buttons;

    private void Start()
    {
        _buttonsCanvasGroup.alpha = 0f;
        _buttonsCanvasGroup.interactable = false;

        _title.rectTransform.DOAnchorPosY(_titleFinalYPosition, _titleTweenDuration).SetEase(Ease.InOutSine);
        _title.rectTransform.DOScale(_titleFinalScale, _titleTweenDuration).SetEase(Ease.InOutSine).OnComplete(ShowButtons);
    }

    private void ShowButtons()
    {
        _buttonsCanvasGroup.DOFade(1, _buttonsTweenDuration).OnComplete(() => _buttonsCanvasGroup.interactable = true);
    }

    public void NewGame()
    {
        //TODO
        //DO THIS WHENEVER LOADING TO OTHER SCENE
        DOTween.KillAll();
        SceneManager.LoadScene(GameData.INTRO_SCENE);
    }

    public void Continue()
    {
        NewGame();
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
