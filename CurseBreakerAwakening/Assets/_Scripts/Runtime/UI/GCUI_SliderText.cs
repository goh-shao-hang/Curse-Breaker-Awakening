using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCells.UI
{
    public class GCUI_SliderText : MonoBehaviour
    {
        [SerializeField] private bool _showInteger = true;
        [SerializeField] private Slider slider;

        private TMP_Text _valueText;

        private void Awake()
        {
            _valueText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            OnSliderValueChanged(slider.value);
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _valueText.text = _showInteger ? value.ToString("0") : value.ToString("0.00");
        }
    }
}