using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private TextMeshProUGUI _textMeshProUGUI;
    public RectTransform RectTransform {  get; private set; }

    public const float _tweenDuration = 0.2f;
    public const float _selectedScale = 1.1f;
    public const float _selectedAlpha = 1f;
    public const float _deselectedAlpha = 0.5f;

    public event Action OnSelected;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _textMeshProUGUI.DOKill();
        _textMeshProUGUI.rectTransform.DOScale(_selectedScale, _tweenDuration);
        _textMeshProUGUI.alpha = _selectedAlpha;

        OnSelected?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _textMeshProUGUI.DOKill();
        _textMeshProUGUI.rectTransform.DOScale(1f, _tweenDuration);
        _textMeshProUGUI.alpha = _deselectedAlpha;
    }
}
