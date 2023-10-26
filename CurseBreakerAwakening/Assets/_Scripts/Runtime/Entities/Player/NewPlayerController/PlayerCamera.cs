using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerInputHandler _playerInputHandler;
        [SerializeField] private Transform _orientationTransform;
        [SerializeField] private Transform _cameraFollowTargetTransform;

        [Header(GameData.SETTINGS)]
        [SerializeField] private float _xSensitivity;
        [SerializeField] private float _ySensitivity;
        [SerializeField] private float _maxVerticalAngle = 80f;

        private float xRotation;
        private float yRotation;

        private float _mouseX;
        private float _mouseY;

        private LTDescr _fovTween = null;

        private void Awake()
        {
            //TODO
            LeanTween.reset();
        }

        private void Start()
        {
            LockAndHideCursor(true);
        }

        private static void LockAndHideCursor(bool lockAndHide)
        {
            Cursor.lockState = lockAndHide ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockAndHide;
        }

        private void Update()
        {
            ReadInput();
            MoveAndRotateCamera();
        }

        private void ReadInput()
        {
            _mouseX = _playerInputHandler.LookInput.x * _xSensitivity;
            _mouseY = _playerInputHandler.LookInput.y * _ySensitivity;
        }

        private void MoveAndRotateCamera()
        {
            //Move
            transform.position = _cameraFollowTargetTransform.position;

            //Rotate
            yRotation += _mouseX;
            xRotation -= _mouseY;
            xRotation = Mathf.Clamp(xRotation, -_maxVerticalAngle, _maxVerticalAngle);

            _camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            _orientationTransform.rotation = Quaternion.Euler(0, yRotation, 0f);
        }

    #region Extra Functions

        public void SetFieldOfView(float fov, float duration = 0.2f)
        {
            if (_fovTween != null)
            {
                LeanTween.cancel(_camera.gameObject, _fovTween.uniqueId);
            }

            _fovTween = LeanTween.value(_camera.gameObject, _camera.fieldOfView, fov, duration).setOnUpdate(value => _camera.fieldOfView = value);
        }
    }

    #endregion
}