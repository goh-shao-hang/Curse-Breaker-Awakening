using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace CBA.LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private GameObject[] _rooms;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2Int _boardSize;
        [SerializeField] private int _maxIterations = 1000;

        [Header(GameData.DEBUG)]
        [SerializeField] private Vector2 _roomOffset;

        //TODO change
        [SerializeField] private Vector2Int _startPosition = Vector2Int.zero;

        Cell[,] _board;

        private void Start()
        {
            Generate();
            TestSpawnRooms();
        }

        private void Generate()
        {
            //Board Initialization
            _board = new Cell[_boardSize.x, _boardSize.y];
            for (int i = 0; i < _boardSize.x; i++)
            {
                for (int j = 0; j < _boardSize.y; j++)
                {
                    _board[i, j] = new Cell();
                }
            }

            Stack<Vector2Int> path = new Stack<Vector2Int>();

            int iterations = 0;
            Vector2Int currentCell = _startPosition;

            while (iterations <= _maxIterations) //Or any exit condition that might be desired
            {
                iterations++;

                _board[currentCell.x, currentCell.y].Visited = true;

                //TODO
                //Feel free to spawn default room or add artificial wait here for visualization, but keep in mind not all connections are established yet
                var room = Instantiate(_rooms[0], new Vector3(currentCell.x * _roomOffset.x, 0, currentCell.y * _roomOffset.y), Quaternion.identity, transform);

                List<Vector2Int> neighbours = CheckNeighbours(currentCell);

                //If no available neighbours to extend towards for current cell, start backtracking
                if (neighbours.Count == 0)
                {
                    if (path.Count == 0)
                    {
                        //Backtracked to the beginning, exit now
                        break;
                    }
                    else
                    {
                        //Backtrack to the previous cell (top of the stack) and check for other neighbours
                        currentCell = path.Pop();
                    }
                }
                //Neighbours found
                else
                {
                    path.Push(currentCell); //Add this cell to the top of the stack

                    //Pick a random neighbour to extend towards
                    Vector2Int nextCell = neighbours[Random.Range(0, neighbours.Count)];

                    //Up
                    if (nextCell.y > currentCell.y)
                    {
                        _board[currentCell.x, currentCell.y].Exits[0] = true; //Up exit is open
                        _board[nextCell.x, nextCell.y].Exits[1] = true;
                    }
                    //Down
                    else if (nextCell.y < currentCell.y)
                    {
                        _board[currentCell.x, currentCell.y].Exits[1] = true; //Down exit is open
                        _board[nextCell.x, nextCell.y].Exits[0] = true;
                    }
                    //Right
                    else if (nextCell.x > currentCell.x)
                    {
                        _board[currentCell.x, currentCell.y].Exits[2] = true; //Right exit is open
                        _board[nextCell.x, nextCell.y].Exits[3] = true;
                    }
                    //Left
                    else
                    {
                        _board[currentCell.x, currentCell.y].Exits[3] = true; //Left exit is open
                        _board[nextCell.x, nextCell.y].Exits[2] = true;
                    }

                    currentCell = nextCell;
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
            if (cell.x - 1 >= 0 && !_board[cell.x - 1, cell.y].Visited)
            {
                neighbours.Add(new Vector2Int(cell.x - 1, cell.y));
            }

            //Right
            if (cell.x + 1 < _boardSize.x && !_board[cell.x + 1, cell.y].Visited)
            {
                neighbours.Add(new Vector2Int(cell.x + 1, cell.y));
            }

            //Up
            if (cell.y - 1 >= 0 && !_board[cell.x, cell.y - 1].Visited)
            {
                neighbours.Add(new Vector2Int(cell.x, cell.y - 1));
            }

            //Down
            if (cell.y + 1 < _boardSize.y && !_board[cell.x, cell.y + 1].Visited)
            {
                neighbours.Add(new Vector2Int(cell.x, cell.y + 1));
            }

            return neighbours;
        }

        private void TestSpawnRooms()
        {
            //Loop through rows
            for (int i = 0; i < _boardSize.x; i++)
            {
                // Loop through columns
                for (int j = 0; j < _boardSize.y; j++)
                {
                    Cell cell = _board[i, j];
                    //cell.UpdateTypeAndRotation();

                    //var room = Instantiate(_rooms[0], new Vector3(i * _roomOffset.x, 0, j * _roomOffset.y), Quaternion.Euler(0f, cell.RoomRotation, 0f), transform).GetComponent<RoomController>();
                    //room.UpdateExits(_board[i, j].Exits);
                }
            }
        }
    }
}