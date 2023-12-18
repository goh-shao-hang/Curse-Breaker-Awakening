using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CBA.LevelGeneration
{
    public class RoomSpawner : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private RoomData[] _roomDatas;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;

        public Dictionary<ERoomShape, GameObject[]> _roomPrefabsDict = new Dictionary<ERoomShape, GameObject[]>();

        private void Awake()
        {
            foreach (var roomData in _roomDatas) 
            {
                if (!_roomPrefabsDict.ContainsKey(roomData.RoomShape))
                {
                    _roomPrefabsDict.Add(roomData.RoomShape, roomData.RoomPrefabs);
                }
            }
        }

        private void OnEnable()
        {
            //_levelGenerator.OnGenerationCompleted += SpawnRooms;
        }

        private void OnDisable()
        {
            //_levelGenerator.OnGenerationCompleted -= SpawnRooms;
        }

        private void SpawnRooms()
        {
            for (int i = 0; i < _levelGenerator.BoardSize.x; i++)
            {
                for (int j = 0; j < _levelGenerator.BoardSize.y; j++)
                {
                    Cell currentCell = _levelGenerator.Board[i, j];
                    if (currentCell.Visited)
                    {
                        ERoomShape roomShape = currentCell.RoomShape;
                        GameObject[] roomPrefabs = _roomPrefabsDict[roomShape];
                        int random = Random.Range(0, roomPrefabs.Length);

                        Instantiate(roomPrefabs[random], new Vector3(i * _roomOffset.x, 0, j * _roomOffset.y), Quaternion.Euler(0f, currentCell.RoomRotation, 0f), transform);
                    }

                }
            }
        }
    }

    [Serializable]
    public class RoomData
    {
        public ERoomShape RoomShape;
        public GameObject[] RoomPrefabs;
    }
}