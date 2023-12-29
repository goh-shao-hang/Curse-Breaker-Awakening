using CBA;
using GameCells.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerGrabManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private PlayerCameraController _playerCameraController;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private PlayerCombatManager _playerCombatManager;
        [SerializeField] private Transform _grabTransform;
        [SerializeField] private MovementModule _movementModule;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _maxGrabDistance = 2f;
        [SerializeField] private float _throwUpwardAdjustment = 0.5f;
        [SerializeField] private LayerMask _interactableLayer;
        [SerializeField] private LayerMask _terrainlayer;

        public bool CanInteract { get; private set; } = true;
        public bool CanGrab => !_playerCombatManager.CurrentWeapon.IsPerformingCombatAction;
        public bool IsGrabbing => _currentGrabbedObject != null;

        private RaycastHit _raycastHit;
        private GrabbableObject _currentGrabbedObject = null;
        private GrabbableObject _currentSelection = null;

        public event Action OnGrab;
        public event Action OnThrow;


        private void OnEnable()
        {
            _playerInputHandler.OnInteractPressedInput += TryGrab;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnInteractPressedInput -= TryGrab;
        }

        private void Update()
        {
            //Manage Highlighting
            if (_currentSelection != null)
            {
                _currentSelection.OnDeselect();
            }
            _currentSelection = null;
            
            //Manage Grabbing
            if (_currentGrabbedObject != null)
            {
                /*//Update grab position
                if (Physics.Raycast(_playerCameraController.PlayerCamera.transform.position, _playerCameraController.PlayerCamera.transform.forward, out _raycastHit,
                    _maxGrabDistance, _terrainlayer))
                {
                    _grabTransform.transform.position = _raycastHit.point;
                }
                else
                {
                    _grabTransform.transform.position = _playerCameraController.PlayerCamera.transform.position
                    + _playerCameraController.PlayerCamera.transform.forward * _maxGrabDistance;
                }*/
            }
            //Only check for highlight if the player is not already grabbing something
            else if (Physics.Raycast(_playerCameraController.PlayerCamera.transform.position, _playerCameraController.PlayerCamera.transform.forward, out _raycastHit, _maxGrabDistance,
                _interactableLayer))
            {
                if (_raycastHit.transform.TryGetComponent(out GrabbableObject grabbableObject))
                {
                    if (grabbableObject.IsGrabbable)
                    {
                        grabbableObject.OnSelect();
                        _currentSelection = grabbableObject;
                    }
                }
            }
        }

        private void TryGrab()
        {
            if (!CanGrab)
                return;

            if (_currentGrabbedObject != null)
            {
                RaycastHit hit;
                Vector3 targetPosition = _playerCameraController.transform.forward * 1000f;
                if (Physics.Raycast(_playerCameraController.transform.position, _playerCameraController.transform.forward, out hit, 1000f))
                {
                    targetPosition = hit.point;
                }

                Vector3 direction = (targetPosition - _grabTransform.position).normalized;

                //_currentGrabbedObject.Throw(_playerCameraController.PlayerCamera.transform.forward);
                _currentGrabbedObject.Throw((direction + Vector3.up * _throwUpwardAdjustment).normalized, Vector3.zero);//TODO _movementModule.CurrentVelocity);
                _currentGrabbedObject = null;

                OnThrow?.Invoke();
            }
            else if (_currentGrabbedObject == null && _currentSelection != null)
            {
                if (_currentSelection.IsGrabbable)
                {
                    _currentSelection.StartGrabbing(_grabTransform, this.transform);
                    _currentGrabbedObject = _currentSelection;

                    _currentSelection.OnDeselect();
                    _currentSelection = null;

                    OnGrab?.Invoke();
                }
            }
        }
    }
}