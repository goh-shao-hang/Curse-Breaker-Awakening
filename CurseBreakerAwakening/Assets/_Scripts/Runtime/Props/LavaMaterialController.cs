using CBA;
using CBA.Entities;
using CBA.LevelGeneration;
using System;
using UnityEngine;

public class LavaMaterialController : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private bool _overrideMaterialSettings;
    [SerializeField] private MaterialSettings _materialSettings;

    private Material _lavaMaterial;
    private void Awake()
    {
        _lavaMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        if (!_overrideMaterialSettings)
            return;

        _lavaMaterial.SetFloat("_NoiseScale", _materialSettings.NoiseScale);
        _lavaMaterial.SetFloat("_NoisePower", _materialSettings.NoisePower);
        _lavaMaterial.SetFloat("_TwirlStrength", _materialSettings.TwirlStrength);
        _lavaMaterial.SetFloat("_DistortionScale", _materialSettings.DistortionScale);
        _lavaMaterial.SetFloat("_DistortionSpeed", _materialSettings.DistortionSpeed);
        _lavaMaterial.SetFloat("_DisplacementScale", _materialSettings.DisplacementScale);
        _lavaMaterial.SetVector("_DisplacementSpeed", _materialSettings.DisplacementSpeed);
        _lavaMaterial.SetFloat("_DisplacementStrength", _materialSettings.DisplacementStrength);
        _lavaMaterial.SetFloat("_FoamOffset", _materialSettings.FoamOffset);
    }

    [Serializable]
    public class MaterialSettings
    {
        public float NoiseScale = 100f;
        public float NoisePower = 4;
        public float TwirlStrength = 1;
        public float DistortionScale = 10;
        public float DistortionSpeed = 0.02f;
        public float DisplacementScale = 5f;
        public Vector2 DisplacementSpeed = Vector2.one * 0.1f;
        public float DisplacementStrength = 1f;
        public float FoamOffset = 1f;
    }
}
