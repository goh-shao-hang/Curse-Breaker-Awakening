using CBA.Entities.Player;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CBA.Entities
{
    public class BossRoomManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private HealthModule _bossHealth;
        [SerializeField] private CanvasGroup _bossCanvasGroup;
        [SerializeField] private Image _healthBarRoot;
        [SerializeField] private Image _healthBarWhite;
        [SerializeField] private Image _healthBarFill;
        [SerializeField] private TMP_Text _victoryText;

        [Header(GameData.SETTINGS)]
        [SerializeField] private int _phases;
        [SerializeField] private float _canvasGroupTween = 1f;
        [SerializeField] private float _shakeStrength = 5f;
        [SerializeField] private int _shakeVibrato = 5;
        [SerializeField] private float _tweenDuration = 0.5f;
        [SerializeField] private float _tweenDelay = 1f;

        private int _currentPhase;

        private void Start()
        {
            _bossCanvasGroup.alpha = 0f;

            //TODO call somewhere else
            StartBossFight();
        }

        private void OnEnable()
        {
            _bossHealth.OnHealthIncreased.AddListener(OnBossHealthIncreased);
            _bossHealth.OnHealthDecreased.AddListener(OnBossHealthDecreased);
        }

        private void OnDisable()
        {
            _bossHealth.OnHealthIncreased.RemoveListener(OnBossHealthIncreased);
            _bossHealth.OnHealthDecreased.RemoveListener(OnBossHealthDecreased);
        }

        public void StartBossFight()
        {
            _currentPhase = 1;

            _bossCanvasGroup.DOFade(1, _canvasGroupTween);
        }

        private void OnBossHealthIncreased()
        {
            float healthPercentage = Mathf.Clamp01(_bossHealth.CurrentHealth / _bossHealth.MaxHealth);

            _healthBarFill.DOFillAmount(healthPercentage, 1f).SetEase(Ease.OutExpo);
            _healthBarWhite.DOFillAmount(healthPercentage, 1f).SetEase(Ease.OutExpo);

        }

        private void OnBossHealthDecreased()
        {
            UpdateBossHealthUI();

            if (_bossHealth.CurrentHealth <= 0)
            {
                if (_currentPhase == _phases) //Last Phase Defeated
                {
                    BossDefeated();
                }
                else
                {
                    NextPhase();
                }
            }
        }

        private void NextPhase()
        {
            _currentPhase++;
        }

        private void BossDefeated()
        {
            _victoryText.rectTransform.DOScale(2, 6f);

            var sequnce = DOTween.Sequence();
            sequnce.Append(_victoryText.DOFade(1, 1f));
            sequnce.AppendInterval(1f);
            sequnce.Append(_victoryText.DOColor(Color.red, 3f));
            sequnce.AppendInterval(1f);
            sequnce.Append(_victoryText.DOFade(0, 1f));
            sequnce.Play();
        }

        private void UpdateBossHealthUI()
        {
            float healthPercentage = Mathf.Clamp01(_bossHealth.CurrentHealth / _bossHealth.MaxHealth);

            _healthBarRoot.rectTransform.DOKill(true);
            _healthBarRoot.rectTransform.DOShakeAnchorPos(0.25f, _shakeStrength, _shakeVibrato, 90, false, false);

            _healthBarFill.fillAmount = healthPercentage;

            _healthBarWhite.DOKill();
            _healthBarWhite.DOFillAmount(healthPercentage, _tweenDuration).SetEase(Ease.OutExpo).SetDelay(_tweenDelay);
        }

        /*private void OnValidate()
        {
            for (int i = 0; i < _phases.Length; i++)
            {
                _phases[i].SetPhaseName(i + 1);
            }
        }*/
    }

    /*[Serializable]
    public class BossPhase
    {
        [HideInInspector] public string PhaseName;

        public UnityEvent OnPhaseEnter;

        public void SetPhaseName(int index)
        {
            PhaseName = $"Phase {index}";
        }
    }*/
}