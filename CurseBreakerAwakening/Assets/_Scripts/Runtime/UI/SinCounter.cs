using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SinCounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _sinPrompt;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _sinPrompt.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _sinPrompt.SetActive(false);
    }
}
