using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class Cell
    {
        public bool Visited = false;
        public bool[] Exits = new bool[4];
    }
}