using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOutlineController : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;

    [Header(GameData.SETTINGS)]
    [ColorUsage(true, true)] [SerializeField] private Color _color;
    [SerializeField] private float _thickness = 0.005f;
    [SerializeField] private bool _startWithOutline = false;

    public const string OUTLINE_NAME = "Mat_3DOutline (Instance)";
    public const string COLOR = "_Color";
    public const string THICKNESS = "_Thickness";
    public const string ACTIVE = "_Active";

    private Material _outlineMaterial;

    private void Awake()
    {
        var materials = _meshRenderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name == OUTLINE_NAME)
            {
                _outlineMaterial = _meshRenderer.materials[i];
                break;
            }
        }

        if (_outlineMaterial == null)
        {
            Debug.LogWarning($"No outline material is found on {gameObject.name}!");
            return;
        }

        _outlineMaterial.SetColor(COLOR, _color);
        _outlineMaterial.SetFloat(THICKNESS, _thickness);
        _outlineMaterial.SetInt(ACTIVE, _startWithOutline ? 1 : 0);
    }

    public void EnableOutline(bool show)
    {
        _outlineMaterial?.SetInt(ACTIVE, show ? 1 : 0);
    }

    public void SetAlpha(float alpha)
    {
        Color color = _outlineMaterial.color;
        color.a = alpha;

        _outlineMaterial.SetColor(COLOR, color);
    }
}
