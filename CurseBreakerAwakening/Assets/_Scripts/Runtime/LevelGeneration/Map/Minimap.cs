using CBA.Core;
using CBA.Entities.Player;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.LevelGeneration
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] private RectTransform _playerIndicatorUI;

        private GameManager _gameManager;
        private GameManager gameManager => _gameManager ??= GameManager.Instance;

        private PlayerCameraController _playerCameraReference => gameManager.PlayerManager?.PlayerCameraController;

        private void Update()
        {
            if (_playerCameraReference == null)
                return;

            Vector3 rotation = _playerCameraReference.transform.rotation.eulerAngles;
            _playerIndicatorUI.rotation = Quaternion.Euler(0f, 0f, -rotation.y);
        }
    }
}