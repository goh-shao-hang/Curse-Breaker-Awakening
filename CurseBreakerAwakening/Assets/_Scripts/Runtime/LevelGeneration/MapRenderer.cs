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
        [SerializeField] private RectTransform _iconContainer;
        [SerializeField] private MapIcon _roomIconPrefab;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;

        private void OnEnable()
        {
            _levelGenerator.OnGenerationCompleted += DrawMap;
        }

        private void OnDisable()
        {
            _levelGenerator.OnGenerationCompleted -= DrawMap;
        }

        private void DrawMap()
        {
            for (int i = 0; i < _levelGenerator.BoardSize.x; i++)
            {
                for (int j = 0; j < _levelGenerator.BoardSize.y; j++)
                {
                    Cell currentCell = _levelGenerator.Board[i, j];
                    if (currentCell.Visited)
                    {
                        //ERoomType roomToSpawn = GetRoomTypeToSpawnFromRules(roomIndex);
                        //Room room = GetRandomRoomOfShapeAndType(currentCell.RoomShape, roomToSpawn);

                        MapIcon roomIcon = Instantiate(_roomIconPrefab, _iconContainer);
                        roomIcon.Image.rectTransform.anchoredPosition = new Vector2(i * _roomOffset.x, j * _roomOffset.y);
                    }
                }
            }
        }
    }
}