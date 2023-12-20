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
        [SerializeField] private RoomTypeData[] _roomTypeDatas;
        [SerializeField] private RoomSpawnerRule[] _rules;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;

        [Header(GameData.DEBUG)]
        [SerializeField] private bool _visualizeGeneration;

        private Dictionary<ERoomType, Dictionary<ERoomShape, List<Room>>> _roomsDict = new Dictionary<ERoomType, Dictionary<ERoomShape, List<Room>>>();

        private Dictionary<Vector2Int, Room> _roomPositionDict = new Dictionary<Vector2Int, Room>();
        private Dictionary<Cell, ERoomType> _roomTypeDict = new Dictionary<Cell, ERoomType>();

        public event Action OnAllRoomsSpawned;

        private void Awake()
        {
            //Dictionary initialization
            foreach (var data in _roomTypeDatas)
            {
                if (!_roomsDict.ContainsKey(data.RoomType))
                {
                    _roomsDict.Add(data.RoomType, new Dictionary<ERoomShape, List<Room>>());

                    foreach (Room room in data.RoomPrefabs)
                    {
                        if (!_roomsDict[data.RoomType].ContainsKey(room.RoomShape))
                        {
                            _roomsDict[data.RoomType].Add(room.RoomShape, new List<Room>());
                        }

                        _roomsDict[data.RoomType][room.RoomShape].Add(room);
                    }
                }
            }
        }

        private void OnEnable()
        {
            if (!_visualizeGeneration)
                _levelGenerator.OnGenerationCompleted += SpawnRooms;
            else
            {
                _levelGenerator.OnCellUpdate += SpawnIndividualRoom;
                _levelGenerator.OnGenerationCompleted += () => OnAllRoomsSpawned?.Invoke();
            }
        }

        private void OnDisable()
        {
            if (!_visualizeGeneration)
                _levelGenerator.OnGenerationCompleted -= SpawnRooms;
            else
            {
                _levelGenerator.OnCellUpdate -= SpawnIndividualRoom;
                _levelGenerator.OnGenerationCompleted -= () => OnAllRoomsSpawned?.Invoke();

            }
        }

        public ERoomType GetRoomType(Cell cell)
        {
            if (_roomTypeDict.ContainsKey(cell))
            {
                return _roomTypeDict[cell];
            }

            return ERoomType.Normal;
        }

        private Room GetRandomRoomOfShapeAndType(ERoomShape shape, ERoomType type)
        {
            List<Room> PossibleRooms = _roomsDict[type][shape];
            return PossibleRooms[Random.Range(0, PossibleRooms.Count)];
        }

        private void SpawnIndividualRoom(Cell cell, Vector2Int cellPosition)
        {
            cell.UpdateTypeAndRotation();

            //Search for relevant rule
            int roomIndex = _levelGenerator.GetCellIndex(cell);

            ERoomType roomToSpawn = GetRoomTypeToSpawnFromRules(roomIndex);
            Room room = GetRandomRoomOfShapeAndType(cell.RoomShape, roomToSpawn);

            if (_roomTypeDict.ContainsKey(cell))
            {
                _roomTypeDict.Remove(cell);
            }
            _roomTypeDict.Add(cell, roomToSpawn);

            if (_roomPositionDict.ContainsKey(cellPosition))
            {
                Destroy(_roomPositionDict[cellPosition].gameObject);
                _roomPositionDict.Remove(cellPosition);
            }

            _roomPositionDict.Add(cellPosition, Instantiate(room, new Vector3(cellPosition.x * _roomOffset.x, 0, cellPosition.y * _roomOffset.y), Quaternion.Euler(0f, cell.RoomRotation, 0f), transform));
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
                        Room room = GetRandomRoomOfShapeAndType(currentCell.RoomShape, roomToSpawn);

                        if (!_roomTypeDict.ContainsKey(currentCell))
                        {
                            _roomTypeDict.Add(currentCell, roomToSpawn);
                        }

                        _roomPositionDict.Add(new Vector2Int(i, j), Instantiate(room, new Vector3(i * _roomOffset.x, 0, j * _roomOffset.y), Quaternion.Euler(0f, currentCell.RoomRotation, 0f), transform));
                    }
                }
            }

            OnAllRoomsSpawned?.Invoke();
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

            foreach (var data in _roomTypeDatas)
            {
                data.UpdateName();
            }
        }
    }

    [Serializable]
    public class RoomTypeData
    {
        [HideInInspector] public string RoomTypeName;

        public ERoomType RoomType = ERoomType.Normal;
        public Room[] RoomPrefabs;

        public void UpdateName()
        {
            RoomTypeName = RoomType.ToString();
        }
    }
}