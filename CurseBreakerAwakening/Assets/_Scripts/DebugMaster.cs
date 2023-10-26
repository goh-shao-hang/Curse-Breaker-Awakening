using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMaster : MonoBehaviour
{
    [Header("Frame Rate")]
    [SerializeField] private int _targetFrameRate = 60;
    [SerializeField] private bool _setOnAwake = true;

#if UNITY_EDITOR

    private void Awake()
    {
        if (_setOnAwake)
        {
            ApplyTargetFrameRate();
        }
    }

    [ContextMenu("Apply Target Frame Rate")]
    public void ApplyTargetFrameRate()
    {
        Application.targetFrameRate = _targetFrameRate;
    }

#endif
}
