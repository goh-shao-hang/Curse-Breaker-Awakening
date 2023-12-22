using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _startup;
    [SerializeField] private RectTransform _menu;

    private void Awake()
    {
    }

    private void Start()
    {
        _startup.gameObject.SetActive(true);
    }

    public void EnableMenu()
    {
        _startup.gameObject.SetActive(false);
        _menu.gameObject.SetActive(true);
    }
}
