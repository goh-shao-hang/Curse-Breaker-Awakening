using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestTween : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] Image flash;
    [SerializeField] private float _endXPos = -450f;
    [SerializeField] private float _rotation = 180f;

    private void Start()
    {
        flash.DOFade(0, _duration).SetEase(Ease.Linear);
        flash.rectTransform.DOScale(0, _duration).SetEase(Ease.OutSine);
        flash.rectTransform.DORotate(new Vector3(0, 0, _rotation), _duration, RotateMode.FastBeyond360).SetEase(Ease.OutSine);
        flash.rectTransform.DOAnchorPosX(_endXPos, _duration);
    }
}
