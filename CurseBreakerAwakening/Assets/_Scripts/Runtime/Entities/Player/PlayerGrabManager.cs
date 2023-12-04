using CBA;
using GameCells.Entities;
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
        [SerializeField] private Transform _grabTransform;
        [SerializeField] private MovementModule _movementModule;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _maxGrabDistance = 2f;
        [SerializeField] private LayerMask _interactableLayer;
        [SerializeField] private LayerMask _terrainlayer;

        public bool CanInteract { get; private set; } = true;

        private RaycastHit _raycastHit;
        private GrabbableObject _currentGrabbedObject = null;
        private GrabbableObject _currentSelection = null;

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
            if (_currentGrabbedObject != null)
            {
                //_currentGrabbedObject.Throw(_playerCameraController.PlayerCamera.transform.forward);
                _currentGrabbedObject.Throw(_grabTransform.forward.normalized + Vector3.up, _movementModule.CurrentVelocity);
                _currentGrabbedObject = null;
            }
            else if (_currentGrabbedObject == null && _currentSelection != null)
            {
                if (_currentSelection.IsGrabbable)
                {
                    _currentSelection.StartGrabbing(_grabTransform, this.transform);
                    _currentGrabbedObject = _currentSelection;

                    _currentSelection.OnDeselect();
                    _currentSelection = null;
                }
            }
        }
    }
}