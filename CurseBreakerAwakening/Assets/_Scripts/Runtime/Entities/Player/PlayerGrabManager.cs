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
        [SerializeField] private LayerMask _damageableLayer;

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

                /*if (_raycastHit.transform.TryGetComponent(out GrabbableObject grabbableObject))
                {
                    if (grabbableObject.IsGrabbable)
                    {
                        grabbableObject.OnSelect();
                        _currentSelection = grabbableObject;
                    }
                }*/
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
            grabbableObject.StartGrabbing(_grabTransform, this.transform);
            _currentGrabbedObject = grabbableObject;

            _currentSelection.OnDeselect();
            _currentSelection = null;

            OnGrab?.Invoke();
        }

        public void Throw()
        {
            Vector3 targetPosition = _playerCameraController.transform.forward * 1000f;
            if (Physics.Raycast(_playerCameraController.transform.position, _playerCameraController.transform.forward, out _raycastHit, 1000f))
            {
                targetPosition = _raycastHit.point;
            }

            Vector3 direction = (targetPosition - _grabTransform.position).normalized;

            _currentGrabbedObject.GetComponentInChildren<ExplosiveModule>()?.SetTargetLayers(_damageableLayer);

            _currentGrabbedObject.Throw((direction + Vector3.up * _throwUpwardAdjustment).normalized, _movementModule.CurrentVelocity);
            _currentGrabbedObject = null;

            OnThrow?.Invoke();
        }
    }
}