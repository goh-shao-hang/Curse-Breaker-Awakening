using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    private TMP_Text _textToScale;
    [SerializeField] private float _start = 1.5f;
    [SerializeField] private float _end = 1;
    [SerializeField] private float _duration = 1;

    private void Start()
    {
        _textToScale = GetComponent<TMP_Text>();

        _textToScale.transform.localScale = Vector3.one * _start;
        _textToScale.rectTransform.DOScale(_end, _duration).SetEase(Ease.OutSine);
    }
}
