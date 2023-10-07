using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class PlayerCameraManager : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;

        private LTDescr _fovTween = null;

        private void Awake()
        {
            //TODO
            LeanTween.reset();
        }

        public void SetFieldOfView(float fov, float duration = 0.2f)
        {
            if (_fovTween != null)
            {
                LeanTween.cancel(_playerCamera.gameObject, _fovTween.uniqueId);
            }

            _fovTween = LeanTween.value(_playerCamera.gameObject, _playerCamera.fieldOfView, fov, duration).setOnUpdate(value => _playerCamera.fieldOfView = value);
        }


    }
}