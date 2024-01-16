using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA
{
    public class InteractableMeshRendererHighlight : MonoBehaviour
    {
        [Header(GameData.SETTINGS)]
        [ColorUsage(true, true)] [SerializeField] private Color _color;
        [SerializeField] private float _thickness = 0.05f;

        private MeshRenderer _meshRenderer;
        private IInteractable _interactable;

        public const string OUTLINE_NAME = "Mat_3DOutline (Instance)";
        public const string COLOR = "_Color";
        public const string THICKNESS = "_Thickness";
        public const string ACTIVE = "_Active";

        private Material _outlineMaterial;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _interactable = GetComponent<IInteractable>();

            for (int i = 0; i < _meshRenderer.materials.Length; i++)
            {
                if (_meshRenderer.materials[i].name == OUTLINE_NAME)
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
            _outlineMaterial.SetInt(ACTIVE, 0);
        }

        private void OnEnable()
        {
            _interactable.OnSelected += Highlight;
            _interactable.OnDeselected += StopHighlight;
        }

        private void OnDisable()
        {
            _interactable.OnSelected -= Highlight;
            _interactable.OnDeselected -= StopHighlight;
        }

        private void Highlight()
        {
            _outlineMaterial?.SetInt(ACTIVE, 1);
        }

        private void StopHighlight()
        {
            _outlineMaterial?.SetInt(ACTIVE, 0);
        }
    }
}