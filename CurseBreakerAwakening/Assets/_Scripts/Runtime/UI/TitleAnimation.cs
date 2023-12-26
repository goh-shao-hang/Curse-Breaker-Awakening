using CBA.Core;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] private MainMenuScene _mainMenu;

    [SerializeField] private Volume _pressStartEffectVolume;
    [SerializeField] private Image _title1White;
    [SerializeField] private Image _title;
    [SerializeField] private Image _titleMaskedImage;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Image _lensFlare;
    [SerializeField] private float _lensFlareRotation = 180f;
    [SerializeField] private float _endXPos = -450f;
    [SerializeField] private TextMeshProUGUI _pressToStart;

    [Header("Particles")]
    [SerializeField] private RectTransform _sparklesParticles;
    [SerializeField] private GameObject _fireParticles;

    private void Start()
    {
        _title1White.rectTransform.localScale = Vector3.one * 1.5f;
        _title.fillAmount = 0f;

        Color transparent = Color.white;
        transparent.a = 0f;
        _title1White.color = transparent;
        _title.color = transparent;
        _lensFlare.color = transparent;

        _pressToStart.alpha = 0;

        _title1White.DOFade(1, 1.5f).SetDelay(0.5f).SetEase(Ease.InOutSine);
        _title1White.rectTransform.DOScale(1, 1.5f).SetDelay(0.5f).SetEase(Ease.OutSine).OnComplete(Title2);

        _fireParticles.SetActive(false);
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

        _sparklesParticles.DOAnchorPosX(_endXPos, _duration).OnComplete(() => _sparklesParticles.GetComponent<ParticleSystem>().Stop());
        _sparklesParticles.GetComponent<ParticleSystem>().Play();

        AudioManager.Instance.PlayGlobalSFX("RevealTitle");

        Invoke(nameof(PressToStartAnimation), 1f);
    }

    private void PressToStartAnimation()
    {
        AudioManager.Instance.PlayBGM("MainMenuTheme");

        _fireParticles.gameObject.SetActive(true);

        _pressToStart.DOFade(1, 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        Invoke(nameof(EnablePressToStart), 1f);
    }

    private IDisposable m_EventListener = null;
    private void EnablePressToStart()
    {
        m_EventListener = InputSystem.onAnyButtonPress.Call(OnPressToStart);
    }

    private GameObject _startEffectWeightReference; //For start effect volume to reference weight since dotween doesnt have punch value
    public async void OnPressToStart(InputControl button)
    {
        m_EventListener.Dispose();

        AudioManager.Instance.PlayGlobalSFX("TitlePressed");
            
        _startEffectWeightReference = new GameObject();
        _startEffectWeightReference.transform.DOPunchPosition(Vector3.right, 1f, 0, 0).SetEase(Ease.OutBounce).OnUpdate(UpdatePressStartEffect);

        await Task.Delay(1000); //Wait 1 second

        _pressToStart.DOKill();
        await _pressToStart.DOFade(0, _duration).SetEase(Ease.InOutSine).AsyncWaitForCompletion();

        _mainMenu.EnableMenu();
    }

    private void UpdatePressStartEffect()
    {
        _pressStartEffectVolume.weight = _startEffectWeightReference.transform.position.x;
    }
}
