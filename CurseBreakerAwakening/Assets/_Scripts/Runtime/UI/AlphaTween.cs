using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlphaTween : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private bool _invert = false;

    private void Start()
    {
        textMeshProUGUI.rectTransform.DOAnchorPosY(textMeshProUGUI.rectTransform.anchoredPosition.y - 150f, 1f).SetDelay(1.5f);
        textMeshProUGUI.DOFade(_invert ? 1 : 0, 1f).SetDelay(1.5f);

    }
}
