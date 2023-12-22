using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu;

    [SerializeField] private Image _title1White;
    [SerializeField] private Image _title;
    [SerializeField] private Image _titleMaskedImage;
    [SerializeField] private float _duration = 1f;
    [FormerlySerializedAs("flash")] [SerializeField] private Image _lensFlare;
    [FormerlySerializedAs("_rotation")][SerializeField] private float _lensFlareRotation = 180f;
    [SerializeField] private float _endXPos = -450f;
    [SerializeField] private TextMeshProUGUI _clickToStart;
    [SerializeField] private RectTransform _particleSystem;
    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioClip _revealSfx;

    private void Start()
    {
        _title1White.rectTransform.localScale = Vector3.one * 1.5f;
        _title.fillAmount = 0f;

        Color transparent = Color.white;
        transparent.a = 0f;
        _title1White.color = transparent;
        _title.color = transparent;
        _lensFlare.color = transparent;

        _clickToStart.alpha = 0;

        _title1White.DOFade(1, 1.5f).SetDelay(0.5f).SetEase(Ease.InOutSine);
        _title1White.rectTransform.DOScale(1, 1.5f).SetDelay(0.5f).SetEase(Ease.OutSine).OnComplete(Title2);
    }

    private void Title2()
    {
        _title1White.DOFillAmount(0, _duration).OnComplete(() => _title1White.gameObject.SetActive(false));
        _title.DOFillAmount(1, _duration);

        Color opaque = Color.white;
        _lensFlare.color = opaque; //Set alpha to 1

        _title.DOFade(1, 1.5f);
        _lensFlare.DOFade(0, _duration).SetEase(Ease.InBounce);
        _lensFlare.rectTransform.DOScale(0, _duration).SetEase(Ease.OutSine);
        _lensFlare.rectTransform.DORotate(new Vector3(0, 0, _lensFlareRotation), _duration, RotateMode.FastBeyond360).SetEase(Ease.OutSine);
        _lensFlare.rectTransform.DOAnchorPosX(_endXPos, _duration);

        Sequence shineSequence = DOTween.Sequence().SetLoops(int.MaxValue);

        shineSequence.Append(_titleMaskedImage.rectTransform.DOAnchorPosX(-750, _duration).SetEase(Ease.OutSine)).AppendInterval(5f);

        _particleSystem.DOAnchorPosX(_endXPos, _duration).OnComplete(() => _particleSystem.GetComponent<ParticleSystem>().Stop());
        _particleSystem.GetComponent<ParticleSystem>().Play();

        _sfxAudioSource.PlayOneShot(_revealSfx);

        Invoke(nameof(PressToStartAnimation), 1f);
    }

    private void PressToStartAnimation()
    {
        _bgmAudioSource.Play();

        _clickToStart.DOFade(1, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        Invoke(nameof(EnablePressToStart), 1f);
    }

    private IDisposable m_EventListener = null;
    private void EnablePressToStart()
    {
        m_EventListener = InputSystem.onAnyButtonPress.Call(OnPressToStart);
    }

    public void OnPressToStart(InputControl button)
    {
        _mainMenu.EnableMenu();

        m_EventListener.Dispose();
    }
}
