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
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private RectTransform _playerIndicatorUI;
        
        private PlayerCameraController _playerCameraReference;

        private void Awake()
        {
            _playerCameraReference = _levelManager.PlayerCameraReference;
        }

        private void Update()
        {
            Vector3 rotation = _playerCameraReference.transform.rotation.eulerAngles;
            _playerIndicatorUI.rotation = Quaternion.Euler(0f, 0f, -rotation.y);
        }
    }
}