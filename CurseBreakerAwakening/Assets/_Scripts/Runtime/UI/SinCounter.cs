using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SinCounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject _sinPrompt;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowPrompt(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowPrompt(false);

    }

    public void OnSelect(BaseEventData eventData)
    {
        ShowPrompt(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ShowPrompt(false);
    }

    private void ShowPrompt(bool show)
    {
        if (_sinPrompt != null)
        {
            _sinPrompt.SetActive(show);
        }
    }
}
