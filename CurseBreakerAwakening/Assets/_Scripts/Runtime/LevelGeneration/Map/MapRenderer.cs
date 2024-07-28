using DG.Tweening;
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
        [SerializeField] private RectTransform _currentRoomIndicator;
        [SerializeField] private Image _indicatorImage1;
        [SerializeField] private Image _indicatorImage2;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;
        [SerializeField] private float _containerPadding = 200f;

        [Header(GameData.DEBUG)]
        [SerializeField] private bool _revealAllOnStart = false;

        private Dictionary<Cell, RoomIcon> _iconDict = new Dictionary<Cell, RoomIcon>();

        private RoomIcon _currentRoomIcon;

        private void Start()
        {
            _indicatorImage1.rectTransform.DORotate(Vector3.forward * 360, 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
            _indicatorImage1.rectTransform.DOScale(0.5f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            _indicatorImage2.rectTransform.DORotate(Vector3.forward * 360, 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        }

        private void OnEnable()
        {
            _roomSpawner.OnAllRoomsSpawned += DrawMap;
        }

        private void OnDestroy()
        {
            _roomSpawner.OnAllRoomsSpawned -= DrawMap;
        }

        private void DrawMap()
        {
            float maxHorizontalSize = 0;
            float maxVerticalSize = 0;

            _iconDict.Clear();

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

                        _iconDict.Add(currentCell, roomIcon);

                        roomIcon.Root.anchoredPosition = new Vector2(i * _roomOffset.x, j * _roomOffset.y);
                        roomIcon.UpdateExits(currentCell);
                        roomIcon.IconImage.sprite = _mapIcons.GetIcon(_roomSpawner.GetRoomType(currentCell));

                        if (!_revealAllOnStart)
                        {
                            if (_levelGenerator.GetCellIndex(currentCell) == 1)
                            {
                                _currentRoomIcon = _iconDict[currentCell];
                                _currentRoomIndicator.anchoredPosition = _currentRoomIcon.Root.anchoredPosition;
                                _currentRoomIcon.gameObject.SetActive(true);
                                _currentRoomIcon.IconImage.rectTransform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                            }
                            else
                            {
                                roomIcon.gameObject.SetActive(false);
                            }
                        }

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

        public void SetCurrentRoom(Cell cell)
        {
            if (_iconDict.ContainsKey(cell))
            {
                _currentRoomIcon?.IconImage.rectTransform.DOKill(true);

                _currentRoomIcon = _iconDict[cell];
                _currentRoomIndicator.anchoredPosition = _currentRoomIcon.Root.anchoredPosition;
                _currentRoomIcon.gameObject.SetActive(true);
                _currentRoomIcon.IconImage.rectTransform.DOScale(1.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }
        }
    }
}