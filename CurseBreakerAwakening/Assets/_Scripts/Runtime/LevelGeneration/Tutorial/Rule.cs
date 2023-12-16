using CBA.LevelGeneration;
using System;
using UnityEngine;

[Serializable]
public class Rule
{
    public RoomController RoomPrefab;
    public Vector2Int MinPosition;
    public Vector2Int MaxPosition;
    public bool obligatory; //Optional if false

    public int GetSpawnChance(int x, int y)
    {
        // 0 - Cannot Spawn, 1 - Can Spawn, 2 - Has to Spawn
        if (x >= MinPosition.x && x <= MaxPosition.x && y >= MinPosition.y && y <= MaxPosition.y)
        {
            return obligatory ? 2 : 1;
        }

        return 0;
    }
}
