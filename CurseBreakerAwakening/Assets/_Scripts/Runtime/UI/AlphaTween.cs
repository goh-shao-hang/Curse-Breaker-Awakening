using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlphaTween : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        textMeshProUGUI.DOFade(0, _duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
