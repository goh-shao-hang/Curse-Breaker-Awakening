using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerSliding : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Transform _orientationTransform;
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private FPPlayerController _playerController;
        [SerializeField] private PlayerInputHandler _playerInputHandler;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _maxSlidingTime;
        [SerializeField] private float _slidingForce;
        [SerializeField] private float _slidingYScale = 0.5f;

        private float _slidingTimer;
        private float _initialYScale;

        private void Start()
        {
            _initialYScale = _playerController.transform.localScale.y;
        }

        private void OnEnable()
        {
            _playerInputHandler.OnSlidePressedInput += OnSlidePressed;
            _playerInputHandler.OnSlideReleasedInput += OnSlideReleased;
        }

        private void OnDisable()
        {
            
        }

        private void FixedUpdate()
        {
            if (_playerController.IsSliding)
                ApplySlidingMovement();
        }

        private void OnSlidePressed()
        {
            if (_playerController.IsSliding)
                return;

            if (_playerInputHandler.MoveInput != Vector2.zero)
            {
                StartSliding();
            }
        }

        private void OnSlideReleased()
        {
            if (!_playerController.IsSliding)
                return;

            StopSliding();
        }

        private void StartSliding()
        {
            _playerController.IsSliding = true;

            _playerController.transform.localScale = new Vector3(_playerController.transform.localScale.x, 
                _slidingYScale, _playerController.transform.localScale.z);

            _playerController.MovementModule.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            _slidingTimer = _maxSlidingTime;
        }

        private void StopSliding()
        {
            _playerController.IsSliding = false;

            _playerController.transform.localScale = new Vector3(_playerController.transform.localScale.x,
               _initialYScale, _playerController.transform.localScale.z);
        }

        private void ApplySlidingMovement()
        {
            Vector3 slideDirection = _orientationTransform.forward * _playerInputHandler.MoveInput.y
                + _orientationTransform.right * _playerInputHandler.MoveInput.x;

            if (!_playerController.IsOnSlope || _playerController.MovementModule.CurrentVelocity.y > -0.1f)
            {
                _playerController.MovementModule.AddForce(slideDirection * _slidingForce);

                _slidingTimer -= Time.fixedDeltaTime;
            }
            else
            {
                _playerController.MovementModule.AddForce(_playerController.ProjectDirectionOnSlope(slideDirection) * _slidingForce);
            }


            if (_slidingTimer <= 0)
            {
                StopSliding();
            }
        }
    }
}