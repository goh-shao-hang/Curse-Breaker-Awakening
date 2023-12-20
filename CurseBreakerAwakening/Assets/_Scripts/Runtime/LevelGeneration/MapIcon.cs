using CBA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    [Header(GameData.DEPENDENCIES)]
    [SerializeField] private Image _image;
    
    public Image Image => _image;
}
