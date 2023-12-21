using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class LevelManager : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private LevelGenerator _levelGenerator;
        [SerializeField] private RoomSpawner _roomSpawner;
        [SerializeField] private MapRenderer _mapRenderer;
        [SerializeField] private GameObject _playerReference;
        [SerializeField] private CanvasGroup _transitionCanvas;

        private const float _roomTransitionDuration = 1f;

        private Dictionary<Cell, Room> _roomsDict;
        private Cell _currentCell;
        private Room _currentRoom => _roomsDict[_currentCell];

        private void OnEnable()
        {
            _roomSpawner.OnAllRoomsSpawned += StartLevel;
        }

        private void OnDisable()
        {
            _roomSpawner.OnAllRoomsSpawned -= StartLevel;
        }

        public void StartLevel()
        {
            _roomsDict = _roomSpawner.RoomsDict;

            foreach (var room in _roomsDict)
            {
                //Activate if is first room
                if (_levelGenerator.GetCellIndex(room.Key) == 1)
                {
                    _currentCell = room.Key;
                    _currentRoom.gameObject.SetActive(true);
                }
                else
                {
                    room.Value.gameObject.SetActive(false);
                }
                
            }

            _playerReference.transform.position = _currentRoom.Exits[0].SpawnPoint.position;
            _playerReference.gameObject.SetActive(true);

            _mapRenderer?.RevealRoom(_currentCell);

            _currentRoom.OnPlayerExitRoom += TransitionToRoom;
        }

        public async void TransitionToRoom(EExitDirection exitDirection)
        {
            //Adjust direction considering rotation of current cell
            int adjustedDirection = ((int)exitDirection + (_currentCell.RoomRotation / 90)) % 4;

            Cell nextCell = GetNextCell(adjustedDirection);
            int entranceDirection = ((adjustedDirection + 2 - (nextCell.RoomRotation / 90))) % 4;
            if (entranceDirection < 0)
            {
                entranceDirection += 4;
            }

            await _transitionCanvas.DOFade(1, _roomTransitionDuration).AsyncWaitForCompletion();

            _currentRoom.gameObject.SetActive(false);
            _currentRoom.OnPlayerExitRoom -= TransitionToRoom;

            EExitDirection nextRoomEntrance = (EExitDirection)entranceDirection;

            _currentCell = nextCell;
            _currentRoom.gameObject.SetActive(true);
            _mapRenderer?.RevealRoom(_currentCell);

            Exit entrance = _currentRoom.GetEntrance(nextRoomEntrance);
            _playerReference.transform.SetPositionAndRotation(entrance.SpawnPoint.position, entrance.SpawnPoint.rotation);
            //_playerReference.gameObject.SetActive(true);

            _currentRoom.OnPlayerExitRoom += TransitionToRoom;

            _transitionCanvas.DOFade(0, _roomTransitionDuration);
        }

        private Cell GetNextCell(int direction)
        {
            switch (direction)
            {
                case 0:
                    return _levelGenerator.Board[_currentCell.Position.x, _currentCell.Position.y + 1];
                case 1:
                    return _levelGenerator.Board[_currentCell.Position.x + 1, _currentCell.Position.y];
                case 2:
                    return _levelGenerator.Board[_currentCell.Position.x, _currentCell.Position.y - 1];
                case 3:
                    return _levelGenerator.Board[_currentCell.Position.x - 1, _currentCell.Position.y];
                default:
                    return _currentCell;
            }
        }
    }
}