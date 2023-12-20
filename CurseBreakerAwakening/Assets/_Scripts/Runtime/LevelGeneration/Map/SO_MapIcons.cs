using CBA.LevelGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Map Icons")]
public class SO_MapIcons : ScriptableObject
{
    [SerializeField] private MapIcon[] icons;

    private Dictionary<ERoomType, Sprite> _iconsDict = new Dictionary<ERoomType, Sprite>();

    private void Awake()
    {
        UpdateDictionary();
    }

    private void UpdateDictionary()
    {
        _iconsDict.Clear();
        foreach (var icon in icons)
        {
            if (!_iconsDict.ContainsKey(icon.RoomType))
            {
                _iconsDict.Add(icon.RoomType, icon.Sprite);
            }
        }
    }

    public Sprite GetIcon(ERoomType roomType)
    {
        return _iconsDict[roomType];
    }

    private void OnValidate()
    {
        foreach (var item in icons)
        {
            item.UpdateName();
        }

        UpdateDictionary();

    }


    [Serializable]
    public class MapIcon
    {
        [HideInInspector] public string IconName;

        public ERoomType RoomType;
        public Sprite Sprite;

        public void UpdateName()
        {
            IconName = RoomType.ToString();
        }
    }
}
