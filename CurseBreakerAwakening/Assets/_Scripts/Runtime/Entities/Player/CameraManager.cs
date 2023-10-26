using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities.Player
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;

        public Camera Camera => _playerCamera;

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
                LeanTween.cancel(Camera.gameObject, _fovTween.uniqueId);
            }

            _fovTween = LeanTween.value(Camera.gameObject, Camera.fieldOfView, fov, duration).setOnUpdate(value => Camera.fieldOfView = value);
        }


    }
}