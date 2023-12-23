using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using DG.Tweening;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private List<MainMenuButton> _buttons;
    [FormerlySerializedAs("_scrollRect")][SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentRect;

    private void OnEnable()
    {
        foreach (var button in _buttons)
        {
            button.OnSelected += () => ScrollToButton(button);
        }

        _buttons[0].GetComponent<Button>().Select();
    }


    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.OnSelected += () => ScrollToButton(button);
        }
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