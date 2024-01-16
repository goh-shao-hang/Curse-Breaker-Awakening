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
        [SerializeField] private PlayerHUDManager _playerHUDManager;
        [SerializeField] private Transform _grabTransform;
        [SerializeField] private MovementModule _movementModule;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _entityInfoCheckDistance = 5f;
        [SerializeField] private float _sphereCastRadius = 1f;
        [SerializeField] private float _maxGrabDistance = 2f;
        [SerializeField] private LayerMask _interactableLayer;
        [SerializeField] private LayerMask _damageableLayer;
        [SerializeField] private LayerMask _throwTargetLayer;

        public bool CanInteract => !_playerCombatManager.CurrentWeapon.IsPerformingCombatAction;
        public bool IsGrabbing => _currentGrabbedObject != null;

        private RaycastHit _raycastHit;
        private IInteractable _currentSelection = null;
        private GrabbableObject _currentGrabbedObject = null;

        public event Action OnGrab;
        public event Action OnThrow;


        private void OnEnable()
        {
            _playerInputHandler.OnInteractPressedInput += OnInteractPressed;
        }

        private void OnDisable()
        {
            _playerInputHandler.OnInteractPressedInput -= OnInteractPressed;
        }

        private void Update()
        {
            //Manage Highlighting
            if (_currentSelection != null)
            {
                _currentSelection.OnDeselect();
            }
            _currentSelection = null;
            
            //TODO
            //Manage Showing Entity Info
            if (Physics.Raycast(_playerCameraController.PlayerCamera.transform.position, _playerCameraController.PlayerCamera.transform.forward, out _raycastHit, _entityInfoCheckDistance,
                _interactableLayer))
            {
                if (_raycastHit.transform.TryGetComponent(out EntityInfo entityInfo))
                {
                    _playerHUDManager.ShowEntityInfo(entityInfo);
                }
                else
                {
                    _playerHUDManager.HideEntityInfo();
                }
            }
            else
            {
                if (Physics.SphereCast(_playerCameraController.PlayerCamera.transform.position, _sphereCastRadius, _playerCameraController.PlayerCamera.transform.forward, out _raycastHit, _entityInfoCheckDistance,
                _interactableLayer))
                {
                    if (_raycastHit.transform.TryGetComponent(out EntityInfo entityInfo))
                    {
                        _playerHUDManager.ShowEntityInfo(entityInfo);
                    }
                    else
                    {
                        _playerHUDManager.HideEntityInfo();
                    }
                }
                else
                {
                    _playerHUDManager.HideEntityInfo();
                }
            }

            //Manage selection
            //Only check for highlight if the player is not already grabbing something
            if (!IsGrabbing && Physics.Raycast(_playerCameraController.PlayerCamera.transform.position, _playerCameraController.PlayerCamera.transform.forward, out _raycastHit, _maxGrabDistance,
                _interactableLayer))
            {
                if (_raycastHit.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnSelect();
                    _currentSelection = interactable;
                }
            }
            else
            {
                if (Physics.SphereCast(_playerCameraController.PlayerCamera.transform.position, _sphereCastRadius, _playerCameraController.PlayerCamera.transform.forward, out _raycastHit, _maxGrabDistance,
                _interactableLayer))
                {
                    if (_raycastHit.transform.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.OnSelect();
                        _currentSelection = interactable;
                    }
                }
            }
        }

        private void OnInteractPressed()
        {
            if (!CanInteract)
                return;

            if (IsGrabbing)
            {
                //Can only throw
                Throw();
            }
            else if (_currentSelection != null)
            {
                //Can grab / interact
                _currentSelection.OnInteract(this);
            }
        }

        public void Grab(GrabbableObject grabbableObject)
        {
            grabbableObject.StartGrabbing(_grabTransform);
            _currentGrabbedObject = grabbableObject;

            _currentSelection.OnDeselect();
            _currentSelection = null;

            OnGrab?.Invoke();
        }

        public void Throw()
        {
            Vector3 targetPosition = _playerCameraController.transform.forward * 1000f;
            if (Physics.Raycast(_playerCameraController.transform.position, _playerCameraController.transform.forward, out _raycastHit, 1000f, _throwTargetLayer))
            {
                targetPosition = _raycastHit.point;
            }

            Vector3 direction = (targetPosition - _grabTransform.position).normalized;

            _currentGrabbedObject.GetComponentInChildren<ExplosiveModule>()?.SetTargetLayers(_damageableLayer);

            _currentGrabbedObject.Throw((direction).normalized, _movementModule.CurrentVelocity);
            _currentGrabbedObject = null;

            OnThrow?.Invoke();
        }
    }
}