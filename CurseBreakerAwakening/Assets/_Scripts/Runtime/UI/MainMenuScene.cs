using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    [SerializeField] private RectTransform _startup;
    [SerializeField] private RectTransform _menu;
    [SerializeField] private TMP_Text _versionNumber;

    private void Start()
    {
        _startup.gameObject.SetActive(true);
        _menu.gameObject.SetActive(false);

        _versionNumber.text = $"v{Application.version} DEMO";
    }

    public void EnableMenu()
    {
        _startup.gameObject.SetActive(false);
        _menu.gameObject.SetActive(true);
    }
}
