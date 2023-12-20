using CBA;
using CBA.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomIcon : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [field: SerializeField] public RectTransform Root;
    [field: SerializeField] public Image IconImage;
    [SerializeField] private Image[] _exitImages;

    public void UpdateExits(Cell cell)
    {
        for (int i = 0; i < cell.Exits.Length; i++)
        {
            _exitImages[i].gameObject.SetActive(cell.Exits[i]);
        }
    }
}
