using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.LevelGeneration
{
    public class LevelCell
    {
        public bool Visited = false;
        public bool[] Exits = new bool[4]; //Up -> Down -> Right -> Left
    }
}