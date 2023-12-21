using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using GameCells.Utilities;
using UnityEngine.Serialization;

namespace CBA.LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header(GameData.SETTINGS)]
        [field: SerializeField] public Vector2Int BoardSize;
        [SerializeField] private Vector2Int _startPosition = Vector2Int.zero;
        [SerializeField] private int _maxIterations = 1000;
        [SerializeField] private int _maxRooms = 25;
        [SerializeField] private bool _allowLoops = true;
        [SerializeField] private bool _isolateFirstRoom = true;

        [Header(GameData.DEBUG)]
        [SerializeField] private float _delay;
        private float _startTime; //For debug

        public event Action<Cell> OnCellUpdate;
        public event Action OnGenerationCompleted;

        public Cell[,] Board { get; private set; }
        private Dictionary<Cell, int> _cellIndexDict = new Dictionary<Cell, int>();

        private Vector2Int _currentCell;
        private Stack<Vector2Int> _path = new Stack<Vector2Int>();

        private int _roomCount = 0;

        private void Start()
        {
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            _startTime = Time.time;

            //Board Initialization
            Board = new Cell[BoardSize.x, BoardSize.y];
            for (int i = 0; i < BoardSize.x; i++)
            {
                for (int j = 0; j < BoardSize.y; j++)
                {
                    Board[i, j] = new Cell(i, j);
                }
            }

            int iterations = 0;
            _currentCell = _startPosition;

            while (iterations <= _maxIterations) //Hard iteration limit to prevent endless looping
            {
                iterations++;

                if (!Board[_currentCell.x, _currentCell.y].Visited)
                {
                    _roomCount++;

                    //Assign index
                    _cellIndexDict.Add(Board[_currentCell.x, _currentCell.y], _roomCount);

                    OnCellUpdate?.Invoke(Board[_currentCell.x, _currentCell.y]);
                }

                //Board[_currentCell.x, _currentCell.y].Room.UpdateExits(Board[_currentCell.x, _currentCell.y].Exits);
                Board[_currentCell.x, _currentCell.y].Visited = true;

                if (_roomCount >= _maxRooms)
                    break; //End iteration

                yield return WaitHandler.GetWaitForSeconds(_delay);

                List<Vector2Int> neighbours = CheckNeighbours(_currentCell);

                //If no available neighbours to extend towards for current cell, start backtracking
                if (neighbours.Count == 0)
                {
                    if (_path.Count == 0)
                    {
                        //Backtracked to the beginning, exit now
                        break;
                    }
                    else
                    {
                        //Backtrack to the previous cell (top of the stack) and check for other neighbours
                        _currentCell = _path.Pop();
                    }
                }
                //Neighbours found
                else
                {
                    _path.Push(_currentCell); //Add this cell to the top of the stack

                    //Pick a random neighbour to extend towards
                    Vector2Int nextCell = neighbours[Random.Range(0, neighbours.Count)];

                    //Up
                    if (nextCell.y > _currentCell.y)
                    {
                        Board[_currentCell.x, _currentCell.y].Exits[0] = true; //Up exit is open
                        Board[nextCell.x, nextCell.y].Exits[2] = true;
                    }
                    //Down
                    else if (nextCell.y < _currentCell.y)
                    {
                        Board[_currentCell.x, _currentCell.y].Exits[2] = true; //Down exit is open
                        Board[nextCell.x, nextCell.y].Exits[0] = true;
                    }
                    //Right
                    else if (nextCell.x > _currentCell.x)
                    {
                        Board[_currentCell.x, _currentCell.y].Exits[1] = true; //Right exit is open
                        Board[nextCell.x, nextCell.y].Exits[3] = true;
                    }
                    //Left
                    else
                    {
                        Board[_currentCell.x, _currentCell.y].Exits[3] = true; //Left exit is open
                        Board[nextCell.x, nextCell.y].Exits[1] = true;
                    }
                    
                    OnCellUpdate?.Invoke(Board[_currentCell.x, _currentCell.y]);

                    _currentCell = nextCell;
                }
            }

            OnGenerationCompleted?.Invoke();

            Debug.LogWarning($"GENERATION COMPLETE IN {Time.time - _startTime} SECONDS");
        }

        public void UpdateAllCellTypes()
        {
            for (int i = 0; i < BoardSize.x; i++)
            {
                // Loop through columns
                for (int j = 0; j < BoardSize.y; j++)
                {
                    Cell cell = Board[i, j];
                    if (cell.Visited)
                    {
                        cell.UpdateTypeAndRotation();
                    }
                }
            }
        }


        /// <summary>
        /// Return a list of positions of nearby available cells that are not out of bounds
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<Vector2Int> CheckNeighbours(Vector2Int cell)
        {
            List<Vector2Int> neighbours = new List<Vector2Int>();

            //Each of the following directional checks checks if the adjacent cell to that direction is out of bounds, and if not, if it is not visited before.
            //If both returns true, it is a valid neighbour to step towards.
            //Additional Note: Keep in mind that the positive values on the board goes right and down.

            //Left
            if (cell.x - 1 >= 0)
            {
                if (_allowLoops || !Board[cell.x - 1, cell.y].Visited) //Valid if loop allowed or cell is not yet visited
                {
                    if (!_isolateFirstRoom || _startPosition != new Vector2Int(cell.x - 1, cell.y)) //Not valid if first room is isolated and neighbour is the first room
                        neighbours.Add(new Vector2Int(cell.x - 1, cell.y));
                }
            }

            //Right
            if (cell.x + 1 < BoardSize.x)
            {
                if (_allowLoops || !Board[cell.x + 1, cell.y].Visited)
                {
                    if (!_isolateFirstRoom || _startPosition != new Vector2Int(cell.x + 1, cell.y)) //Not valid if first room is isolated and neighbour is the first room
                        neighbours.Add(new Vector2Int(cell.x + 1, cell.y));
                }
            }

            //Up
            if (cell.y - 1 >= 0)
            {
                if (_allowLoops || !Board[cell.x, cell.y - 1].Visited)
                {
                    if (!_isolateFirstRoom || _startPosition != new Vector2Int(cell.x, cell.y - 1)) //Not valid if first room is isolated and neighbour is the first room
                        neighbours.Add(new Vector2Int(cell.x, cell.y - 1));
                }
            }

            //Down
            if (cell.y + 1 < BoardSize.y)
            {
                if (_allowLoops || !Board[cell.x, cell.y + 1].Visited)
                {
                    if (!_isolateFirstRoom || _startPosition != new Vector2Int(cell.x - 1, cell.y + 1)) //Not valid if first room is isolated and neighbour is the first room
                        neighbours.Add(new Vector2Int(cell.x, cell.y + 1));
                }
            }

            return neighbours;
        }

        public int GetCellIndex(Cell cell)
        {
            return _cellIndexDict[cell];
        }
    }
}