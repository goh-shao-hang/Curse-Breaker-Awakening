using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScene : MonoBehaviour
{
    [SerializeField] private RectTransform _startup;
    [SerializeField] private RectTransform _menu;

    private void Start()
    {
        _startup.gameObject.SetActive(true);
        _menu.gameObject.SetActive(false);
    }

    public void EnableMenu()
    {
        _startup.gameObject.SetActive(false);
        _menu.gameObject.SetActive(true);
    }
}
