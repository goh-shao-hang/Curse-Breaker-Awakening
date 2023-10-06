using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMaster : MonoBehaviour
{
    [Header(GameData.SETTINGS)]
    [SerializeField] private int _targetFrameRate = 60;

#if UNITY_EDITOR

    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
    }

#endif
}
