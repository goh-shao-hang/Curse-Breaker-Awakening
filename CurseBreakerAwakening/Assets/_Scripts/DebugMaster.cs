using CBA;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugMaster : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private TMP_Text _fpsText;

    [Header("Frame Rate")]
    [SerializeField] private int _targetFrameRate = 60;
    [SerializeField] private bool _setOnAwake = true;
    [SerializeField] private bool _displayFPS = false;

#if UNITY_EDITOR

    private float _fps;

    private void Awake()
    {
        if (_setOnAwake)
        {
            ApplyTargetFrameRate();
        }

        _fpsText.gameObject.SetActive(_displayFPS);
    }

    private void Update()
    {
        if (!_displayFPS || _fpsText == null)
            return;

        _fps = 1 / Time.deltaTime;
        _fpsText.text = $"FPS: {Mathf.RoundToInt(_fps)}";
    }

    [ContextMenu("Apply Target Frame Rate")]
    public void ApplyTargetFrameRate()
    {
        Application.targetFrameRate = _targetFrameRate;
    }

#endif
}
