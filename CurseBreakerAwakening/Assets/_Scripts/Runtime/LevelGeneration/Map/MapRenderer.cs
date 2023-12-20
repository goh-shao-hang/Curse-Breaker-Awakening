using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CBA.LevelGeneration
{
    public class MapRenderer : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private RoomSpawner _roomSpawner;
        [SerializeField] private RectTransform _viewport;
        [SerializeField] private RectTransform _iconContainer;
        [SerializeField] private RoomIcon _roomIconPrefab;
        [SerializeField] private SO_MapIcons _mapIcons;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;
        [SerializeField] private float _containerPadding = 200f;

        private void OnEnable()
        {
            _roomSpawner.OnAllRoomsSpawned += DrawMap;
        }

        private void OnDisable()
        {
            _roomSpawner.OnAllRoomsSpawned -= DrawMap;
        }

        private void DrawMap()
        {
            float maxHorizontalSize = 0;
            float maxVerticalSize = 0;

            for (int i = 0; i < _levelGenerator.BoardSize.x; i++)
            {
                for (int j = 0; j < _levelGenerator.BoardSize.y; j++)
                {
                    Cell currentCell = _levelGenerator.Board[i, j];
                    if (currentCell.Visited)
                    {
                        //ERoomType roomToSpawn = GetRoomTypeToSpawnFromRules(roomIndex);
                        //Room room = GetRandomRoomOfShapeAndType(currentCell.RoomShape, roomToSpawn);

                        RoomIcon roomIcon = Instantiate(_roomIconPrefab, _iconContainer);
                        roomIcon.Root.anchoredPosition = new Vector2(i * _roomOffset.x, j * _roomOffset.y);
                        roomIcon.UpdateExits(currentCell);
                        roomIcon.IconImage.sprite = _mapIcons.GetIcon(_roomSpawner.GetRoomType(currentCell));

                        if (j > maxVerticalSize)
                        {
                            maxVerticalSize = j;
                        }

                        if (i > maxHorizontalSize)
                        {
                            maxHorizontalSize = i;
                        }
                    }
                }
            }

            _viewport.sizeDelta = new Vector2(maxHorizontalSize * _roomOffset.x + _containerPadding * 2, maxVerticalSize * _roomOffset.y + _containerPadding* 2);
        }

    }
}