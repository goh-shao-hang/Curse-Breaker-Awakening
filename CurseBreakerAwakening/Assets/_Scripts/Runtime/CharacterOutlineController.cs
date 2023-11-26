using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOutlineController : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private SkinnedMeshRenderer[] _meshRenderers;

    [Header(GameData.SETTINGS)]
    [SerializeField] private float _initialThickness = 0f;
    [SerializeField] private bool _startWithOutline = false;

    public const string OUTLINE_NAME = "3DOutline (Instance)";
    public const string COLOR = "_Color";
    public const string THICKNESS = "_Thickness";
    public const string ACTIVE = "_Active";

    private List<Material> _outlineMaterials = new List<Material>();

    private void Awake()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            var materials = _meshRenderers[i].materials;

            for (int j = 0; j < materials.Length; j++)
            {
                if (materials[j].name == OUTLINE_NAME)
                {
                    _outlineMaterials.Add(materials[j]);
                    break;
                }
            }

            if (_outlineMaterials == null)
            {
                Debug.LogError($"No outline material is found on {gameObject.name}!");
            }
            else
            {
                _outlineMaterials[i].SetFloat(THICKNESS, _initialThickness);

                if (!_startWithOutline)
                {
                    HideOultine();
                }
            }
        }

        
    }

    public void SetThickness(float thickness)
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _outlineMaterials[i].SetFloat(THICKNESS, thickness);
        }
    }

    public void HideOultine()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _outlineMaterials[i].SetInt(ACTIVE, 0);
        }
    }

    public void ShowOultine()
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            _outlineMaterials[i].SetInt(ACTIVE, 1);
        }
    }

    public void SetAlpha(float alpha)
    {
        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            Color color = _outlineMaterials[i].color;
            color.a = alpha;

            _outlineMaterials[i].SetColor(COLOR, color);
        }
    }
}
