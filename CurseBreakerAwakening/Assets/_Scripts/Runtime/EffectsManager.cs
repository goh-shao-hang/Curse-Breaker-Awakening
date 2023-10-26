using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    private Camera _mainCamera;

    private LTDescr _fovTween;

    private void Awake()
    {
        LeanTween.reset();

        _mainCamera = Camera.main;
    }

    public void ChangeFOV(float fov)
    {
        if (_fovTween != null)
        {
            LeanTween.cancel(_mainCamera.gameObject, _fovTween.uniqueId);
        }

        _fovTween = LeanTween.value(_mainCamera.gameObject, _mainCamera.fieldOfView, fov, 0.2f).setOnUpdate(value => _mainCamera.fieldOfView = value);
    }

    public void ChangeFOV(float fov, float duration = 0.2f)
    {
        if (_fovTween != null)
        {
            LeanTween.cancel(_mainCamera.gameObject, _fovTween.uniqueId);
        }

        _fovTween = LeanTween.value(_mainCamera.gameObject, _mainCamera.fieldOfView, fov, duration).setOnUpdate(value => _mainCamera.fieldOfView = value);
    }
}
