using CBA.Core;
using CBA.Entities.Player;
using GameCells.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CBA
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _openVFX;
        [SerializeField] private LootDropModule _lootDropModule;
        [SerializeField] private AudioEmitter _audioEmitter;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _lootDropDelay = 1f;

        public event Action OnSelected;
        public event Action OnDeselected;

        private bool _opened = false;

        private void Start()
        {
            _openVFX.SetActive(false);
        }

        private void OnEnable()
        {
            if (_opened)
            {
                _animator.SetBool(GameData.OPENED_HASH, true);
            }
        }

        public void OnSelect()
        {
            if (_opened)
                return;

            OnSelected?.Invoke();
        }

        public void OnDeselect()
        {
            OnDeselected?.Invoke();
        }

        public void OnInteract(PlayerGrabManager playerGrabManager)
        {
            if (_opened)
                return;

            _opened = true;
            _animator.SetTrigger(GameData.OPEN_HASH);

            StartCoroutine(ChestOpenCO());
        }

        private IEnumerator ChestOpenCO()
        {
            yield return WaitHandler.GetWaitForSeconds(_lootDropDelay);
            _audioEmitter?.PlayOneShotSfx("Chest_Open");
            _openVFX.SetActive(true);
            _lootDropModule.Drop();
        }
    }
}