using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class DungeonGenerator : MonoBehaviour
    {
        [Header(GameData.DEPENDENCIES)]
        [SerializeField] private Rule[] _rooms;

        [Header(GameData.SETTINGS)]
        [SerializeField] private Vector2 _roomOffset;
        [SerializeField] private Vector2Int _boardSize;
        [SerializeField] private int _startPos = 0;
        [SerializeField] private int _maxIterations = 1000;

        private List<Cell> _board;

        void Start()
        {
            Generate();
        }

        private void Generate()
        {
            _board = new List<Cell>();

            for (int i = 0; i < _boardSize.x; i++)
            {
                for (int j = 0; j < _boardSize.y; j++)
                {
                    _board.Add(new Cell());
                }
            }

            //Keep track of the current path of the depth-first algorithm
            Stack<int> path = new Stack<int>();

            int currentCell = _startPos;
            int iterations = 0;

            while (iterations < _maxIterations)
            {
                //Take a step
                iterations++;

                _board[currentCell].Visited = true;

                if (currentCell == _board.Count - 1) //Last Cell on the board
                {
                    break;
                }

                //Check Neighbours
                List<int> neighbours = CheckNeighbours(currentCell);

                if (neighbours.Count == 0) //No available neighbours
                {
                    if (path.Count == 0) //Reached the last cell in the path
                    {
                        break; //Finish iteration
                    }
                    else
                    {
                        currentCell = path.Pop(); //Backtrack and take the last cell added to the path to reiterate
                    }
                }
                else //Available neightbours found
                {
                    path.Push(currentCell); //Add this cell to the path

                    //Choose random available neighbour as next step
                    int nextCell = neighbours[Random.Range(0, neighbours.Count)];

                    if (nextCell > currentCell) //Down or right
                    {
                        if (nextCell - 1 == currentCell)
                        {
                            //Next cell is at right
                            _board[currentCell].Exits[2] = true; //This cell has exit to right
                            currentCell = nextCell;
                            _board[currentCell].Exits[3] = true; //Next cell has exit to left
                        }
                        else
                        {
                            //Next cell is below
                            _board[currentCell].Exits[1] = true; //This cell has exit to down
                            currentCell = nextCell;
                            _board[currentCell].Exits[0] = true; //Next cell has exit to up
                        }
                    }
                    else //Up or left
                    {
                        if (nextCell + 1 == currentCell)
                        {
                            //Next cell is at left
                            _board[currentCell].Exits[3] = true; //This cell has exit to left
                            currentCell = nextCell;
                            _board[currentCell].Exits[2] = true; //Next cell has exit to right
                        }
                        else
                        {
                            //Next cell is above
                            _board[currentCell].Exits[0] = true; //This cell has exit to up
                            currentCell = nextCell;
                            _board[currentCell].Exits[1] = true; //Next cell has exit to down
                        }
                    }
                }
            }

            //After the board is finished, spawn actual rooms
            SpawnRooms();
        }

        private List<int> CheckNeighbours(int cell)
        {
            List<int> neighbours = new List<int>();

            //Check upper cell
            if (cell - _boardSize.x >= 0 && !_board[cell - _boardSize.x].Visited) //The cell above is not out of bounds and not visited
            {
                neighbours.Add(cell - _boardSize.x);
            }

            //Check lower cell
            if (cell + _boardSize.x < _board.Count && !_board[cell + _boardSize.x].Visited) //The cell below is not out of bounds and not visited
            {
                neighbours.Add(cell + _boardSize.x);
            }

            //Check right cell
            if ((cell + 1) % _boardSize.x != 0 && !_board[cell + 1].Visited) //If remainder is 0, this is the last column thus the right cell is out of bounds
            {
                neighbours.Add(cell + 1);
            }

            //Check left cell
            if (cell % _boardSize.x != 0 && !_board[cell - 1].Visited) //If remainder is 0, this is the first column thus the left cell is out of bounds
            {
                neighbours.Add(cell - 1);
            }

            return neighbours;
        }

        private void SpawnRooms()
        {
            for (int i = 0; i < _boardSize.x; i++)
            {
                for (int j = 0; j < _boardSize.y; j++)
                {
                    Cell currentCell = _board[i + j * _boardSize.x];
                    if (currentCell.Visited)
                    {
                        //Spawn room at the grid, with regarding to the offset (offset i times to right and j times downwards)
                        int random = -1;
                        List<int> availableRooms = new List<int>();
                        for (int k = 0; k < _rooms.Length; k++)
                        {
                            int probability = _rooms[k].GetSpawnChance(i, j);

                            if (probability == 2)
                            {
                                random = k; //The first obligatory room at this position will always spawn
                                break;
                            }
                            else if (probability == 1)
                            {
                                availableRooms.Add(k); //Add to a list of room that can be spawned and decide which to spawn later
                            }
                        }

                        if (random == -1) //No obligatory room found
                        {
                            //Choose from any available rooms
                            if (availableRooms.Count > 0)
                            {
                                random = availableRooms[Random.Range(0, availableRooms.Count)];
                            }
                            else //No possible rooms found
                            {
                                Debug.LogWarning($"Default Room spawned at {i},{j}");
                                random = 0; //Use default room instead
                            }
                        }

                        var newRoom = Instantiate(_rooms[random].RoomPrefab, new Vector3(i * _roomOffset.x, 0, -j * _roomOffset.y), Quaternion.identity, transform);
                        newRoom.UpdateExits(currentCell.Exits); //Update the room regarding which exits should be open

                        newRoom.name += $" {i}-{j}";
                    }
                }
            }
        }
    }
}