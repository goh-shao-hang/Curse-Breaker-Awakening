using CBA.Core;
using CBA.Entities.Player;
using CBA.LevelGeneration;
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
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Entity _bossEntity;
        [SerializeField] private Portal _portalToNextLevel;

        [Header("UI")]
        [SerializeField] private CanvasGroup _bossHealthCanvasGroup;
        [SerializeField] private CanvasGroup _victoryTextCanvasGroup;
        [SerializeField] private Image _healthBarRoot;
        [SerializeField] private Image _healthBarWhite;
        [SerializeField] private Image _healthBarFill;
        [SerializeField] private TMP_Text _bossName;
        [SerializeField] private TMP_Text _victoryText1;
        [SerializeField] private TMP_Text _victoryText2;
        [SerializeField] private TMP_Text _portalPrompt;
        [SerializeField] private GameObject _textSpacingReference;
        [SerializeField] private string _bossBGMName;

        [Header(GameData.SETTINGS)]
        [SerializeField] private int _phases;
        [SerializeField] private float _canvasGroupTween = 1f;
        [SerializeField] private float _shakeStrength = 5f;
        [SerializeField] private int _shakeVibrato = 5;
        [SerializeField] private float _tweenDuration = 0.5f;
        [SerializeField] private float _tweenDelay = 1f;

        private int _currentPhase;
        private bool _bossFightStarted = false;

        private PlayerManager _playerManager;

        private HealthModule _bossHealth;

        private void OnTriggerEnter(Collider other)
        {
            if (_bossFightStarted) return;

            if (((1 << other.gameObject.layer) & GameData.PLAYER_LAYER) != 0)
            {
                StartBossFight();
            }
        }

        private void Start()
        {
            _playerManager = GameManager.Instance.PlayerManager;
            _playerManager.ActivateComponents(true);
            _playerManager.PlayerController.transform.position = _playerSpawnPoint.position;
            _playerManager.PlayerCameraController.SetCameraRotation(_playerSpawnPoint.rotation.eulerAngles.y, _playerSpawnPoint.rotation.eulerAngles.x);


            _portalToNextLevel.gameObject.SetActive(false);

            _bossName.text = _bossEntity.EntityData.EntityName;

            _bossHealthCanvasGroup.alpha = 0f;
            _victoryTextCanvasGroup.alpha = 0f;
            _portalPrompt.alpha = 0f;

            _textSpacingReference.transform.position = Vector3.zero;
        }

        private void OnEnable()
        {
            if (_bossHealth == null)
                _bossHealth = _bossEntity.GetModule<HealthModule>();

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
            _bossFightStarted = true;

            _currentPhase = 1;

            _bossHealthCanvasGroup.DOFade(1, _canvasGroupTween);

            AudioManager.Instance.PlayBGM(_bossBGMName);
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
            var sequnce = DOTween.Sequence();

            sequnce.Append(_bossHealthCanvasGroup.DOFade(0, _canvasGroupTween));
            sequnce.Join(_victoryTextCanvasGroup.DOFade(1, 2f));
            sequnce.Join(_textSpacingReference.transform.DOLocalMoveX(23, 5f).OnUpdate(UpdateVictoryTextSpacing));
            sequnce.Join(_portalPrompt.DOFade(1, 1f).SetDelay(1f)).OnPlay(ActivatePortalToNextLevel);
            sequnce.Append(_victoryTextCanvasGroup.DOFade(0, 2f));

            sequnce.Play();

            AudioManager.Instance.CrossFadeBGM("VictoryTheme", 2f, false);
        }

        private void ActivatePortalToNextLevel()
        {
            _portalToNextLevel.gameObject.SetActive(true);
        }

        private void UpdateBossHealthUI()
        {
            float healthPercentage = Mathf.Clamp01(_bossHealth.CurrentHealth / _bossHealth.MaxHealth);

            _healthBarRoot.rectTransform.DOKill(true);
            _healthBarRoot.rectTransform.DOShakeAnchorPos(0.25f, _shakeStrength, _shakeVibrato, 90, false, false);

            _healthBarFill.fillAmount = healthPercentage;

            _healthBarWhite.DOKill();
            _healthBarWhite.DOFillAmount(healthPercentage, _tweenDuration).SetEase(Ease.OutExpo).SetDelay(_tweenDelay).OnComplete(() => _healthBarWhite.fillAmount = healthPercentage);
        }

        private void UpdateVictoryTextSpacing()
        {
            _victoryText1.characterSpacing = _textSpacingReference.transform.position.x;
            _victoryText2.characterSpacing = _textSpacingReference.transform.position.x;
            _portalPrompt.characterSpacing = _textSpacingReference.transform.position.x;
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