using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestTween : MonoBehaviour
{
    [SerializeField] private Image _title1;
    [SerializeField] private Image _title2;
    [SerializeField] private float _duration = 1f;
    [SerializeField] Image flash;
    [SerializeField] private float _endXPos = -450f;
    [SerializeField] private float _rotation = 180f;
    [SerializeField] private TextMeshProUGUI _clickToStart;

    private void Start()
    {
        _title1.rectTransform.localScale = Vector3.one * 1.5f;
        _title2.fillAmount = 0f;

        Color color = Color.white;
        color.a = 0f;
        _title1.color = color;
        _title2.color = color;
        flash.color = color;

        _clickToStart.alpha = 0;


        _title1.DOFade(1, 1.5f).SetDelay(0.5f).SetEase(Ease.InOutSine);
        _title1.rectTransform.DOScale(1, 1.5f).SetDelay(0.5f).SetEase(Ease.OutSine).OnComplete(Title2);
    }

    private void Title2()
    {
        _title2.DOFillAmount(1, _duration);

        Color color = Color.white;
        flash.color = color; //Set alpha to 1

        _title2.DOFade(1, 1.5f);
        flash.DOFade(0, _duration).SetEase(Ease.InBounce);
        flash.rectTransform.DOScale(0, _duration).SetEase(Ease.OutSine);
        flash.rectTransform.DORotate(new Vector3(0, 0, _rotation), _duration, RotateMode.FastBeyond360).SetEase(Ease.OutSine);
        flash.rectTransform.DOAnchorPosX(_endXPos, _duration);

        Invoke(nameof(ClickToStart), 1f);
    }

    private void ClickToStart()
    {
        _clickToStart.DOFade(1, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
