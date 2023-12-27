using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using DG.Tweening;

public class ButtonScrollList : MonoBehaviour
{
    [SerializeField] private List<MainMenuButton> _buttons;
    [SerializeField] private CanvasGroup _buttonsCanvasGroup;
    [FormerlySerializedAs("_scrollRect")][SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentRect;

    private const float _buttonsTweenDuration = 1f;

    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            button.OnSelected += () => ScrollToButton(button);
        }

        EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.OnSelected += () => ScrollToButton(button);
        }
    }

    public void ShowButtons()
    {
        _buttonsCanvasGroup.DOFade(1, _buttonsTweenDuration).OnComplete(() => _buttonsCanvasGroup.interactable = true);
    }

    public void SetInteractable(bool interactable)
    {
        _buttonsCanvasGroup.interactable = interactable;
    }

    public void DisableButtons()
    {
        _buttonsCanvasGroup.alpha = 0f;
        _buttonsCanvasGroup.interactable = false;
    }


    private Tween _scrollTween = null;
    public void ScrollToButton(MainMenuButton button)
    {
        float targetValue = 1 - (float)_buttons.IndexOf(button) / _buttons.Count;

        if (_scrollTween != null)
            _scrollTween.Kill();

        _scrollTween = DOTween.To(() => _scrollRect.verticalNormalizedPosition, x => _scrollRect.verticalNormalizedPosition = x, targetValue, 0.2f);

    }
}