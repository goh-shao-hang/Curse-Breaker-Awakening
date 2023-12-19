using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CBA.LevelGeneration
{
    public class RoomSpawner : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private RoomData[] _roomDatas;
        [SerializeField] private RoomSpawnerRule[] _rules;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;

        [Header(GameData.DEBUG)]
        [SerializeField] private bool _visualizeGeneration;

        private Dictionary<Cell, GameObject> RoomsDict = new Dictionary<Cell, GameObject>();
        public Dictionary<ERoomShape, GameObject[]> _roomPrefabsDict = new Dictionary<ERoomShape, GameObject[]>();

        private void Awake()
        {
            foreach (var roomData in _roomDatas) 
            {
                if (!_roomPrefabsDict.ContainsKey(roomData.RoomShape))
                {
                    _roomPrefabsDict.Add(roomData.RoomShape, roomData.Rooms[0].RoomPrefabs);
                }
            }
        }

        private void OnEnable()
        {
            _levelGenerator.OnGenerationCompleted += SpawnRooms;

            if (_visualizeGeneration)
                _levelGenerator.OnCellUpdate += SpawnIndividualRoom;
        }

        private void OnDisable()
        {
            _levelGenerator.OnGenerationCompleted -= SpawnRooms;

            if (_visualizeGeneration)
                _levelGenerator.OnCellUpdate -= SpawnIndividualRoom;
        }

        private void SpawnIndividualRoom(Cell cell, Vector2Int cellPosition)
        {
            cell.UpdateTypeAndRotation();
            ERoomShape roomShape = cell.RoomShape;
            GameObject[] roomPrefabs = _roomPrefabsDict[roomShape];
            int random = Random.Range(0, roomPrefabs.Length);

            cell.Room = Instantiate(roomPrefabs[random], new Vector3(cellPosition.x * _roomOffset.x, 0, cellPosition.y * _roomOffset.y), Quaternion.Euler(0f, cell.RoomRotation, 0f), transform);
        }

        private void SpawnRooms()
        {
            for (int i = 0; i < _levelGenerator.CellIndexDict.Count; i++)
            {

            }

            /*for (int i = 0; i < _levelGenerator.BoardSize.x; i++)
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
            }*/
        }

        private void OnValidate()
        {
            foreach (var rule in _rules)
            {
                rule.UpdateName();
            }

            foreach (var data in _roomDatas)
            {
                data.UpdateName();

                foreach (var room in data.Rooms)
                {
                    room.UpdateName();
                }
            }
        }
    }

    [Serializable]
    public class RoomData
    {
        [HideInInspector] public string ShapeName;

        public ERoomShape RoomShape;
        public Rooms[] Rooms;

        public void UpdateName()
        {
            ShapeName = RoomShape.ToString();
        }
    }

    [Serializable]
    public class Rooms
    {
        [HideInInspector] public string RoomTypeName;

        public ERoomType RoomType;
        public GameObject[] RoomPrefabs;

        public void UpdateName()
        {
            RoomTypeName = RoomType.ToString();
        }
    }
}