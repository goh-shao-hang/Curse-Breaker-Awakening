using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CBA.LevelGeneration
{
    public class RoomSpawner : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private RoomShapeData[] _roomDatas;
        [SerializeField] private RoomSpawnerRule[] _rules;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;

        [Header(GameData.DEBUG)]
        [SerializeField] private bool _visualizeGeneration;

        private Dictionary<ERoomShape, Dictionary<ERoomType, GameObject[]>> _roomsDict = new Dictionary<ERoomShape, Dictionary<ERoomType, GameObject[]>>();

        //For visualization only
        private Dictionary<Cell, GameObject> RoomsDict = new Dictionary<Cell, GameObject>();


        private void Awake()
        {
            //Dictionary initialization
            foreach (var data in _roomDatas)
            {
                if (!_roomsDict.ContainsKey(data.RoomShape))
                {
                    _roomsDict.Add(data.RoomShape, new Dictionary<ERoomType, GameObject[]>());

                    foreach (var types in data.RoomTypes)
                    {
                        if (!_roomsDict[data.RoomShape].ContainsKey(types.RoomType))
                        {
                            _roomsDict[data.RoomShape].Add(types.RoomType, types.RoomPrefabs);
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            if (!_visualizeGeneration)
                _levelGenerator.OnGenerationCompleted += SpawnRooms;
            else
                _levelGenerator.OnCellUpdate += SpawnIndividualRoom;
        }

        private void OnDisable()
        {
            if (!_visualizeGeneration)
                _levelGenerator.OnGenerationCompleted -= SpawnRooms;
            else
                _levelGenerator.OnCellUpdate -= SpawnIndividualRoom;
        }

        private GameObject GetRandomRoomOfShapeAndType(ERoomShape shape, ERoomType type)
        {
            GameObject[] PossibleRooms = _roomsDict[shape][type];
            return PossibleRooms[Random.Range(0, PossibleRooms.Length)];
        }

        private void SpawnIndividualRoom(Cell cell, Vector2Int cellPosition)
        {
            cell.UpdateTypeAndRotation();

            //Search for relevant rule
            int roomIndex = _levelGenerator.GetCellIndex(cell);

            ERoomType roomToSpawn = GetRoomTypeToSpawnFromRules(roomIndex);
            GameObject room = GetRandomRoomOfShapeAndType(cell.RoomShape, roomToSpawn);

            if (RoomsDict.ContainsKey(cell))
            {
                Destroy(RoomsDict[cell]);
                RoomsDict.Remove(cell);
            }

            RoomsDict.Add(cell, Instantiate(room, new Vector3(cellPosition.x * _roomOffset.x, 0, cellPosition.y * _roomOffset.y), Quaternion.Euler(0f, cell.RoomRotation, 0f), transform));
        }

        
        private void SpawnRooms()
        {
            _levelGenerator.UpdateAllCellTypes();

            for (int i = 0; i < _levelGenerator.BoardSize.x; i++)
            {
                for (int j = 0; j < _levelGenerator.BoardSize.y; j++)
                {
                    Cell currentCell = _levelGenerator.Board[i, j];
                    if (currentCell.Visited)
                    {
                        //Search for relevant rule
                        int roomIndex = _levelGenerator.GetCellIndex(currentCell);

                        ERoomType roomToSpawn = GetRoomTypeToSpawnFromRules(roomIndex);
                        GameObject room = GetRandomRoomOfShapeAndType(currentCell.RoomShape, roomToSpawn);

                        Instantiate(room, new Vector3(i * _roomOffset.x, 0, j * _roomOffset.y), Quaternion.Euler(0f, currentCell.RoomRotation, 0f), transform);
                    }
                }
            }
        }

        private ERoomType GetRoomTypeToSpawnFromRules(int roomIndex)
        {
            ERoomType roomToSpawn = ERoomType.Normal;

            List<RoomSpawnerRule> rulesSatisfied = new List<RoomSpawnerRule>();
            float totalSpawnChance = 0f;
            foreach (var rule in _rules)
            {
                if (roomIndex >= rule.MinSpawnIndex && roomIndex <= rule.MaxSpawnIndex)
                {
                    rulesSatisfied.Add(rule);
                    totalSpawnChance += rule.SpawnChance;
                }
            }

            float random = Random.Range(0, totalSpawnChance);
            float cumulativeChance = 0f;
            foreach (var rule in rulesSatisfied)
            {
                cumulativeChance += rule.SpawnChance;

                if (random <= cumulativeChance)
                {
                    roomToSpawn = rule.roomType;
                    break;
                }
            }

            return roomToSpawn;
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

                foreach (var room in data.RoomTypes)
                {
                    room.UpdateName();
                }
            }
        }
    }

    [Serializable]
    public class RoomShapeData
    {
        [HideInInspector] public string ShapeName;

        public ERoomShape RoomShape;
        [FormerlySerializedAs("Rooms")] public RoomTypeData[] RoomTypes;

        public void UpdateName()
        {
            ShapeName = RoomShape.ToString();
        }
    }

    [Serializable]
    public class RoomTypeData
    {
        [HideInInspector] public string RoomTypeName;

        public ERoomType RoomType = ERoomType.Normal;
        public GameObject[] RoomPrefabs;

        public void UpdateName()
        {
            RoomTypeName = RoomType.ToString();
        }
    }
}